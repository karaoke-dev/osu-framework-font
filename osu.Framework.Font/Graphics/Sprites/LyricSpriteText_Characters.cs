// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Layout;
using osu.Framework.Text;
using osuTK;

namespace osu.Framework.Graphics.Sprites
{
    public partial class LyricSpriteText
    {
        private readonly LayoutValue charactersCache = new LayoutValue(Invalidation.DrawSize | Invalidation.Presence, InvalidationSource.Parent);

        /// <summary>
        /// Glyph list to be passed to <see cref="TextBuilder"/>.
        /// </summary>
        private readonly List<TextBuilderGlyph> charactersBacking = new List<TextBuilderGlyph>();

        /// <summary>
        /// The characters in local space.
        /// </summary>
        private IReadOnlyList<TextBuilderGlyph> characters
        {
            get
            {
                computeCharacters();
                return charactersBacking;
            }
        }

        /// <summary>
        /// Compute character textures and positions.
        /// </summary>
        private void computeCharacters()
        {
            // Note : this feature can only use in osu-framework
            // if (LoadState >= LoadState.Loaded)
            //     ThreadSafety.EnsureUpdateThread();

            if (store == null)
                return;

            if (charactersCache.IsValid)
                return;

            charactersBacking.Clear();

            // Todo: Re-enable this assert after autosize is split into two passes.
            // Debug.Assert(!isComputingCharacters, "Cyclic invocation of computeCharacters()!");

            Vector2 textBounds = Vector2.Zero;

            try
            {
                if (string.IsNullOrEmpty(displayedText))
                    return;

                TextBuilder textBuilder = getTextBuilder();

                textBuilder.Reset();
                textBuilder.AddText(displayedText);
                textBounds = textBuilder.Bounds;

                if (rubies?.Any() ?? false)
                {
                    var rubyTextBuilder = CreateRubyTextBuilder(store);
                    rubies.ForEach(x => rubyTextBuilder.AddText(x));
                }

                if (romajies?.Any() ?? false)
                {
                    var romajiTextBuilder = CreateRomajiTextBuilder(store);
                    romajies.ForEach(x => romajiTextBuilder.AddText(x));
                }
            }
            finally
            {
                if (requiresAutoSizedWidth)
                    base.Width = textBounds.X + Padding.Right;

                if (requiresAutoSizedHeight)
                {
                    var romajiHeight = ReserveRomajiHeight || (Romajies?.Any() ?? false) ? RomajiFont.Size : 0;
                    base.Height = textBounds.Y + romajiHeight + Padding.Bottom;
                }

                base.Width = Math.Min(base.Width, MaxWidth);

                charactersCache.Validate();
            }
        }

        private readonly LayoutValue parentScreenSpaceCache = new LayoutValue(Invalidation.DrawSize | Invalidation.Presence | Invalidation.DrawInfo, InvalidationSource.Parent);
        private readonly LayoutValue localScreenSpaceCache = new LayoutValue(Invalidation.MiscGeometry, InvalidationSource.Self);

        private readonly List<ScreenSpaceCharacterPart> screenSpaceCharactersBacking = new List<ScreenSpaceCharacterPart>();

        /// <summary>
        /// The characters in screen space. These are ready to be drawn.
        /// </summary>
        private IEnumerable<ScreenSpaceCharacterPart> screenSpaceCharacters
        {
            get
            {
                computeScreenSpaceCharacters();
                return screenSpaceCharactersBacking;
            }
        }

        private void computeScreenSpaceCharacters()
        {
            if (!parentScreenSpaceCache.IsValid)
            {
                localScreenSpaceCache.Invalidate();
                parentScreenSpaceCache.Validate();
            }

            if (localScreenSpaceCache.IsValid)
                return;

            screenSpaceCharactersBacking.Clear();

            Vector2 inflationAmount = DrawInfo.MatrixInverse.ExtractScale().Xy;

            foreach (var character in characters)
            {
                screenSpaceCharactersBacking.Add(new ScreenSpaceCharacterPart
                {
                    DrawQuad = ToScreenSpace(character.DrawRectangle.Inflate(inflationAmount)),
                    InflationPercentage = Vector2.Divide(inflationAmount, character.DrawRectangle.Size),
                    Texture = character.Texture
                });
            }

            localScreenSpaceCache.Validate();
        }

        public float GetTextIndexPosition(TextIndex index)
        {
            int charIndex = Math.Clamp(index.Index, 0, Text.Length - 1);
            if (charIndex != index.Index)
                throw new ArgumentOutOfRangeException(nameof(index));

            var characterRectangle = characters[charIndex].DrawRectangle;
            var computedRectangle = getComputeCharacterDrawRectangle(characterRectangle);
            return index.State == TextIndex.IndexState.Start ? computedRectangle.Left : computedRectangle.Right;
        }

        public RectangleF GetCharacterRectangle(int index)
        {
            int charIndex = Math.Clamp(index, 0, Text.Length - 1);
            if (charIndex != index)
                throw new ArgumentOutOfRangeException(nameof(index));

            var character = characters[charIndex];
            var drawRectangle = character.DrawRectangle;
            return getComputeCharacterDrawRectangle(drawRectangle);
        }

        public RectangleF GetRubyTagPosition(PositionText rubyTag)
        {
            int rubyIndex = Rubies.ToList().IndexOf(rubyTag);
            if (rubyIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(rubyIndex));

            int startCharacterIndex = Text.Length + skinIndex(Rubies, rubyIndex);
            int count = rubyTag.Text.Length;
            var drawRectangle = characters.ToList()
                                          .GetRange(startCharacterIndex, count)
                                          .Select(x => x.DrawRectangle)
                                          .Aggregate(RectangleF.Union);
            return getComputeCharacterDrawRectangle(drawRectangle);
        }

        public RectangleF GetRomajiTagPosition(PositionText romajiTag)
        {
            int romajiIndex = Romajies.ToList().IndexOf(romajiTag);
            if (romajiIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(romajiIndex));

            int startCharacterIndex = Text.Length + skinIndex(Rubies, Rubies.Count) + skinIndex(Romajies, romajiIndex);
            int count = romajiTag.Text.Length;
            var drawRectangle = characters.ToList()
                                          .GetRange(startCharacterIndex, count)
                                          .Select(x => x.DrawRectangle)
                                          .Aggregate(RectangleF.Union);
            return getComputeCharacterDrawRectangle(drawRectangle);
        }

        private int skinIndex(IEnumerable<PositionText> positionTexts, int endIndex)
            => positionTexts.Where((_, i) => i < endIndex).Sum(x => x.Text.Length);

        private RectangleF getComputeCharacterDrawRectangle(RectangleF originalCharacterDrawRectangle)
        {
            // combine the rectangle to get the max value.
            return Shaders.OfType<IApplicableToCharacterSize>()
                          .Select(x => x.ComputeCharacterDrawRectangle(originalCharacterDrawRectangle))
                          .Aggregate(originalCharacterDrawRectangle, RectangleF.Union);
        }
    }
}
