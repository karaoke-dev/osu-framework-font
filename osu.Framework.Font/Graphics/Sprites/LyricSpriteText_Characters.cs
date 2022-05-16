// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
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

                var fixedRubies = getFixedPositionText(rubies, displayedText);
                var fixedRomajies = getFixedPositionText(romajies, displayedText);

                if (fixedRubies.Any())
                {
                    var rubyTextBuilder = CreateRubyTextBuilder(store);
                    fixedRubies.ForEach(x => rubyTextBuilder.AddText(x));
                }

                if (fixedRomajies.Any())
                {
                    var romajiTextBuilder = CreateRomajiTextBuilder(store);
                    fixedRomajies.ForEach(x => romajiTextBuilder.AddText(x));
                }
            }
            finally
            {
                if (requiresAutoSizedWidth)
                    base.Width = textBounds.X + Padding.Right;

                if (requiresAutoSizedHeight)
                {
                    var romajiHeight = ReserveRomajiHeight || Romajies.Any() ? RomajiFont.Size : 0;
                    base.Height = textBounds.Y + romajiHeight + Padding.Bottom;
                }

                base.Width = Math.Min(base.Width, MaxWidth);

                charactersCache.Validate();
            }

            static List<PositionText> getFixedPositionText(IEnumerable<PositionText> positionTexts, string lyricText)
                => positionTexts
                   .Where(x => !string.IsNullOrEmpty(x.Text))
                   .Select(x => GetFixedPositionText(x, lyricText))
                   .ToList();
        }

        internal static PositionText GetFixedPositionText(PositionText positionText, string lyricText)
        {
            var startIndex = Math.Clamp(positionText.StartIndex, 0, lyricText.Length);
            var endIndex = Math.Clamp(positionText.EndIndex, 0, lyricText.Length);
            return new PositionText(positionText.Text, Math.Min(startIndex, endIndex), Math.Max(startIndex, endIndex));
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
            var computedRectangle = GetCharacterRectangle(index.Index);
            return index.State == TextIndex.IndexState.Start ? computedRectangle.Left : computedRectangle.Right;
        }

        public RectangleF GetCharacterRectangle(int index, bool drawSizeOnly = false)
        {
            int charIndex = Math.Clamp(index, 0, Text.Length - 1);
            if (charIndex != index)
                throw new ArgumentOutOfRangeException(nameof(index));

            var character = characters[charIndex];
            var drawRectangle = getCharacterRectangle(character, drawSizeOnly);
            return getComputeCharacterDrawRectangle(drawRectangle);
        }

        public RectangleF GetRubyTagPosition(PositionText rubyTag, bool drawSizeOnly = false)
        {
            int rubyIndex = Rubies.ToList().IndexOf(rubyTag);
            if (rubyIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(rubyIndex));

            int startCharacterIndex = Text.Length + skinIndex(Rubies, rubyIndex);
            int count = rubyTag.Text.Length;
            var drawRectangle = characters.ToList()
                                          .GetRange(startCharacterIndex, count)
                                          .Select(x => getCharacterRectangle(x, drawSizeOnly))
                                          .Aggregate(RectangleF.Union);
            return getComputeCharacterDrawRectangle(drawRectangle);
        }

        public RectangleF GetRomajiTagPosition(PositionText romajiTag, bool drawSizeOnly = false)
        {
            int romajiIndex = Romajies.ToList().IndexOf(romajiTag);
            if (romajiIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(romajiIndex));

            int startCharacterIndex = Text.Length + skinIndex(Rubies, Rubies.Count) + skinIndex(Romajies, romajiIndex);
            int count = romajiTag.Text.Length;
            var drawRectangle = characters.ToList()
                                          .GetRange(startCharacterIndex, count)
                                          .Select(x => getCharacterRectangle(x, drawSizeOnly))
                                          .Aggregate(RectangleF.Union);
            return getComputeCharacterDrawRectangle(drawRectangle);
        }

        private static RectangleF getCharacterRectangle(TextBuilderGlyph character, bool drawSizeOnly)
        {
            if (drawSizeOnly)
                return character.DrawRectangle;

            // todo: should get the real value.
            var topReduce = character.Baseline * 0.3f;
            var bottomIncrease = character.Baseline * 0.2f;
            return character.DrawRectangle.Inflate(new MarginPadding
            {
                Top = character.YOffset - topReduce,
                Bottom = character.Baseline - character.Height - character.YOffset + bottomIncrease,
            });
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
