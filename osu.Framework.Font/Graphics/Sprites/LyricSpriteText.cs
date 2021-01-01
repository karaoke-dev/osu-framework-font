// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Text;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Sprites
{
    public class LyricSpriteText : SpriteText, IHasRuby, IHasRomaji, IHasTexture
    {
        public LyricSpriteText()
        {
            Font = new FontUsage(null, 48);
        }

        private PositionText[] rubies;

        public PositionText[] Rubies
        {
            get => rubies;
            set
            {
                rubies = filterValidValues(value);
                Invalidate(Invalidation.All);
            }
        }

        private PositionText[] romajies;

        public PositionText[] Romajies
        {
            get => romajies;
            set
            {
                romajies = filterValidValues(value);
                Invalidate(Invalidation.All);
            }
        }

        private PositionText[] filterValidValues(PositionText[] texts)
        {
            string text = Text;
            return texts?.Where(positionText => Math.Min(positionText.StartIndex, positionText.EndIndex) >= 0
                                                && Math.Max(positionText.StartIndex, positionText.EndIndex) <= text.Length
                                                && positionText.EndIndex > positionText.StartIndex).ToArray();
        }

        private FontUsage rubyFont = FontUsage.Default;

        /// <summary>
        /// Contains information on the font used to display the text.
        /// </summary>
        public FontUsage RubyFont
        {
            get => rubyFont;
            set
            {
                rubyFont = value;
                Invalidate(Invalidation.All);
            }
        }

        private FontUsage romajiFont = FontUsage.Default;

        /// <summary>
        /// Contains information on the font used to display the text.
        /// </summary>
        public FontUsage RomajiFont
        {
            get => romajiFont;
            set
            {
                romajiFont = value;
                Invalidate(Invalidation.All);
            }
        }

        private int rubyMargin;

        public int RubyMargin
        {
            get => rubyMargin;
            set
            {
                if (rubyMargin == value)
                    return;

                rubyMargin = value;
                Invalidate(Invalidation.All);
            }
        }

        private int romajiMargin;

        public int RomajiMargin
        {
            get => romajiMargin;
            set
            {
                if (romajiMargin == value)
                    return;

                romajiMargin = value;
                Invalidate(Invalidation.All);
            }
        }

        private Vector2 rubySpacing;

        public Vector2 RubySpacing
        {
            get => rubySpacing;
            set
            {
                if (rubySpacing == value)
                    return;

                rubySpacing = value;
                Invalidate(Invalidation.All);
            }
        }

        private Vector2 romajiSpacing;

        public Vector2 RomajiSpacing
        {
            get => romajiSpacing;
            set
            {
                if (romajiSpacing == value)
                    return;

                romajiSpacing = value;
                Invalidate(Invalidation.All);
            }
        }

        private ILyricTexture textTexture;

        public ILyricTexture TextTexture
        {
            get => textTexture;
            set
            {
                if (textTexture == value)
                    return;

                textTexture = value;
                Colour = (textTexture as SolidTexture)?.SolidColor ?? Color4.White;
                Invalidate(Invalidation.All);
            }
        }

        private ILyricTexture shadowTexture;

        public ILyricTexture ShadowTexture
        {
            get => shadowTexture;
            set
            {
                if (shadowTexture == value)
                    return;

                shadowTexture = value;
                ShadowColour = (shadowTexture as SolidTexture)?.SolidColor ?? Color4.White;
                Invalidate(Invalidation.All);
            }
        }

        private ILyricTexture borderTexture;

        public ILyricTexture BorderTexture
        {
            get => borderTexture;
            set
            {
                if (borderTexture == value)
                    return;

                borderTexture = value;
                Invalidate(Invalidation.All);
            }
        }

        private LyricTextAlignment rubyAlignment;

        public LyricTextAlignment RubyAlignment
        {
            get => rubyAlignment;
            set
            {
                if (rubyAlignment == value)
                    return;

                rubyAlignment = value;
                Invalidate(Invalidation.All);
            }
        }

        private LyricTextAlignment romajiAlignment;

        public LyricTextAlignment RomajiAlignment
        {
            get => romajiAlignment;
            set
            {
                if (romajiAlignment == value)
                    return;

                romajiAlignment = value;
                Invalidate(Invalidation.All);
            }
        }

        private float borderRadius;

        public float BorderRadius
        {
            get => borderRadius;
            set
            {
                if (borderRadius == value)
                    return;

                borderRadius = value;
                Invalidate(Invalidation.All);
            }
        }

        private bool border;

        public bool Border
        {
            get => border;
            set
            {
                if (border == value)
                    return;

                border = value;
                Invalidate(Invalidation.All);
            }
        }

        public new Vector2 ShadowOffset
        {
            get => base.ShadowOffset * Font.Size;
            set => base.ShadowOffset = value / Font.Size;
        }

        // Store characters without ruby and romaji
        protected TextBuilderGlyph[] Characters;

        /// <summary>
        /// Creates a <see cref="TextBuilder"/> to generate the character layout for this <see cref="SpriteText"/>.
        /// </summary>
        /// <param name="store">The <see cref="ITexturedGlyphLookupStore"/> where characters should be retrieved from.</param>
        /// <returns>The <see cref="TextBuilder"/>.</returns>
        protected override TextBuilder CreateTextBuilder(ITexturedGlyphLookupStore store)
        {
            const int builder_max_width = int.MaxValue;

            var excludeCharacters = FixedWidthExcludeCharacters;

            // Calculate position
            var rubyYPosition = Padding.Top;
            var contentPosition = rubyYPosition + RubyFont.Size / 2 + RubyMargin;

            // Print and save main texts
            var charactersBacking = createMainTexts(Text, Font, contentPosition, Spacing);
            Characters = charactersBacking.ToArray();

            // Print ruby texts
            createTexts(Rubies, RubyFont, rubyYPosition, RubySpacing);

            // Calculate position and print romaji texts
            var romajiYPosition = contentPosition + Characters.FirstOrDefault().Height + Characters.FirstOrDefault().YOffset + RomajiMargin;
            createTexts(Romajies, RomajiFont, romajiYPosition, RomajiSpacing);

            // Calculate position and return TextBuilder that do not renderer text anymore
            var romajiTextSize = RomajiMargin + ((Romajies?.Any() ?? false) ? (charactersBacking.LastOrDefault().Height + charactersBacking.LastOrDefault().YOffset) : 0);
            return new TextBuilder(store, Font, builder_max_width, UseFullGlyphHeight,
                new Vector2(Padding.Left, contentPosition + romajiTextSize), Spacing, null,
                excludeCharacters, FallbackCharacter);

            // Create main text
            List<TextBuilderGlyph> createMainTexts(string text, FontUsage font, float yPosition, Vector2 spacing)
            {
                var existCharacters = base.CreateTextBuilder(store).Characters;

                var builder = new TextBuilder(store, Font, builder_max_width, UseFullGlyphHeight,
                    new Vector2(Padding.Left, yPosition), spacing, existCharacters,
                    excludeCharacters, FallbackCharacter);

                builder.AddText(text);

                return builder.Characters;
            }

            // Create ruby and romaji texts
            void createTexts(PositionText[] positionTexts, FontUsage font, float yPosition, Vector2 spacing)
            {
                if (positionTexts != null)
                {
                    foreach (var positionText in positionTexts)
                    {
                        var text = positionText.Text;
                        if (string.IsNullOrEmpty(text))
                            continue;

                        // Get text position
                        var textPosition = new Vector2(getTextPosition(positionText, spacing.X), yPosition);

                        var builder = new TextBuilder(store, font, builder_max_width, UseFullGlyphHeight,
                            textPosition, spacing, charactersBacking, excludeCharacters, FallbackCharacter);

                        builder.AddText(text);
                    }
                }

                // Convert
                float getTextPosition(PositionText text, float textSpacing)
                {
                    //It's magic number to let text in the center
                    const float size_multiple = 1.4f;
                    var centerPosition = (Characters[text.StartIndex].DrawRectangle.Left + Characters[text.EndIndex - 1].DrawRectangle.Right) / 2;
                    var textWidth = text.Text.Sum(c => (store.Get(font.FontName, c)?.Width ?? 0) * font.Size * size_multiple) - (text.Text.Length) * textSpacing;
                    return centerPosition - textWidth / 2;
                }
            }
        }

        public float GetPercentageWidth(TextIndex startIndex, TextIndex endIndex, float percentage = 0)
        {
            if (Characters == null)
                return 0;

            var charLength = Characters.Length;
            if (charLength == 0)
                return 0;

            startIndex = TimeTagIndexUtils.Clamp(startIndex, 0, charLength - 1);
            endIndex = TimeTagIndexUtils.Clamp(endIndex, 0, charLength - 1);

            var left = getWidth(startIndex);
            var right = getWidth(endIndex);

            var width = left * (1 - percentage) + right * percentage;
            return width + Margin.Left;

            float getWidth(TextIndex timeTagIndex)
            {
                switch (timeTagIndex.State)
                {
                    case TextIndex.IndexState.Start:
                        return Characters[timeTagIndex.Index].DrawRectangle.Left;
                    case TextIndex.IndexState.End:
                        return Characters[timeTagIndex.Index].DrawRectangle.Right;
                    default:
                        throw new InvalidOperationException(nameof(timeTagIndex.State));
                }
            }
        }
    }
}
