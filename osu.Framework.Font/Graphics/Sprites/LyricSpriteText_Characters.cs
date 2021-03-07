// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Layout;
using osu.Framework.Text;
using osu.Framework.Utils;
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
        private List<TextBuilderGlyph> Characters
        {
            get
            {
                computeCharacters();
                return charactersBacking;
            }
        }

        private bool isComputingCharacters;

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

            Debug.Assert(!isComputingCharacters, "Cyclic invocation of computeCharacters()!");
            isComputingCharacters = true;

            TextBuilder textBuilder = null;

            try
            {
                if (string.IsNullOrEmpty(displayedText))
                    return;

                textBuilder = CreateTextBuilder(store);
                textBuilder.AddText(displayedText);

                // Get main text position and main text array.
                var existCharacter = textBuilder.Characters.ToArray();

                // Print ruby texts
                var rubyYPosition = Padding.Top - RubyMargin;
                createPositionTexts(existCharacter, Rubies, rubyYPosition, true);

                // Calculate position and print romaji texts
                var textHeight = existCharacter.FirstOrDefault().Height + existCharacter.FirstOrDefault().YOffset;
                var romajiYPosition = textHeight + RomajiMargin;
                createPositionTexts(existCharacter, Romajies, romajiYPosition, false);
            }
            finally
            {
                if (requiresAutoSizedWidth)
                    base.Width = (textBuilder?.Bounds.X ?? 0) + Padding.Right;
                if (requiresAutoSizedHeight)
                    base.Height = (textBuilder?.Bounds.Y ?? 0) + Padding.Bottom;

                base.Width = Math.Min(base.Width, MaxWidth);

                isComputingCharacters = false;
                charactersCache.Validate();
            }

            // Create ruby and romaji texts
            void createPositionTexts(TextBuilderGlyph[] mainTexts, PositionText[] positionTexts, float yPosition, bool ruby)
            {
                positionTexts?.ForEach(p =>
                {
                    var text = p.Text;
                    if (string.IsNullOrEmpty(text))
                        return;

                    // Get text position
                    var spacing = ruby ? rubySpacing : romajiSpacing;
                    var textPosition = new Vector2(getTextPosition(p, spacing.X), yPosition);

                    // create ruby or romaji builder
                    var builder = ruby ? CreateRubyTextBuilder(store, textPosition) : CreateRomajiTextBuilder(store, textPosition);
                    builder.AddText(text);
                });

                // Convert
                float getTextPosition(PositionText text, float textSpacing)
                {
                    //It's magic number to let text in the center
                    const float size_multiple = 1.4f;
                    var centerPosition = (mainTexts[text.StartIndex].DrawRectangle.Left + mainTexts[text.EndIndex - 1].DrawRectangle.Right) / 2;
                    var textWidth = text.Text.Sum(c => (store.Get(font.FontName, c)?.Width ?? 0) * font.Size * size_multiple) - (text.Text.Length) * textSpacing;
                    return centerPosition - textWidth / 2;
                }
            }
        }

        private readonly LayoutValue parentScreenSpaceCache = new LayoutValue(Invalidation.DrawSize | Invalidation.Presence | Invalidation.DrawInfo, InvalidationSource.Parent);
        private readonly LayoutValue localScreenSpaceCache = new LayoutValue(Invalidation.MiscGeometry, InvalidationSource.Self);

        private readonly List<ScreenSpaceCharacterPart> screenSpaceCharactersBacking = new List<ScreenSpaceCharacterPart>();

        /// <summary>
        /// The characters in screen space. These are ready to be drawn.
        /// </summary>
        private List<ScreenSpaceCharacterPart> screenSpaceCharacters
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

            foreach (var character in Characters)
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

        private readonly LayoutValue<Vector2> shadowOffsetCache = new LayoutValue<Vector2>(Invalidation.DrawInfo, InvalidationSource.Parent);

        private Vector2 premultipliedShadowOffset =>
            shadowOffsetCache.IsValid ? shadowOffsetCache.Value : shadowOffsetCache.Value = ToScreenSpace(shadowOffset * Font.Size) - ToScreenSpace(Vector2.Zero);

        public float GetPercentageWidth(TextIndex startIndex, TextIndex endIndex, float percentage = 0)
        {
            if (Characters == null)
                return 0;

            var charLength = Characters.Count();
            if (charLength == 0)
                return 0;

            startIndex = TextIndexUtils.Clamp(startIndex, 0, charLength - 1);
            endIndex = TextIndexUtils.Clamp(endIndex, 0, charLength - 1);

            var left = getWidth(startIndex);
            var right = getWidth(endIndex);

            var width = left * (1 - percentage) + right * percentage;
            return width + Margin.Left;

            float getWidth(TextIndex index)
            {
                switch (index.State)
                {
                    case TextIndex.IndexState.Start:
                        return Characters[index.Index].DrawRectangle.Left;
                    case TextIndex.IndexState.End:
                        return Characters[index.Index].DrawRectangle.Right;
                    default:
                        throw new InvalidOperationException(nameof(index.State));
                }
            }
        }
    }
}
