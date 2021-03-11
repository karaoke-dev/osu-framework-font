// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace osu.Framework.Text
{
    public class PositionTextBuilder : TextBuilder
    {
        private readonly char fallbackCharacter;
        private readonly ITexturedGlyphLookupStore store;
        private readonly FontUsage font;
        private readonly Vector2 spacing;

        /// <summary>
        /// Creates a new <see cref="TextBuilder"/>.
        /// </summary>
        /// <param name="store">The store from which glyphs are to be retrieved from.</param>
        /// <param name="font">The font to use for glyph lookups from <paramref name="store"/>.</param>
        /// <param name="useFontSizeAsHeight">True to use the provided <see cref="font"/> size as the height for each line. False if the height of each individual glyph should be used.</param>
        /// <param name="startOffset">The offset at which characters should begin being added at.</param>
        /// <param name="spacing">The spacing between characters.</param>
        /// <param name="maxWidth">The maximum width of the resulting text bounds.</param>
        /// <param name="characterList">That list to contain all resulting <see cref="TextBuilderGlyph"/>s.</param>
        /// <param name="neverFixedWidthCharacters">The characters for which fixed width should never be applied.</param>
        /// <param name="fallbackCharacter">The character to use if a glyph lookup fails.</param>
        /// <param name="fixedWidthReferenceCharacter">The character to use to calculate the fixed width width. Defaults to 'm'.</param>
        public PositionTextBuilder(ITexturedGlyphLookupStore store, FontUsage font, float maxWidth, bool useFontSizeAsHeight = true, Vector2 startOffset = default,
                                     Vector2 spacing = default, List<TextBuilderGlyph> characterList = null, char[] neverFixedWidthCharacters = null,
                                     char fallbackCharacter = '?', char fixedWidthReferenceCharacter = 'm', LyricTextAlignment alignment = LyricTextAlignment.Auto)
            : base(store, font, maxWidth, useFontSizeAsHeight, startOffset, spacing, characterList, neverFixedWidthCharacters, fallbackCharacter, fixedWidthReferenceCharacter)
        {
            this.store = store;
            this.font = font;
            this.spacing = spacing;
            this.fallbackCharacter = fallbackCharacter;
        }

        /// <summary>
        /// Appends text to this <see cref="TextBuilder"/>.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void AddText(PositionText positionText)
        {
            var text = positionText.Text;
            if (string.IsNullOrEmpty(text))
                return;

            // calculate start position
            var canterPosition = getCenterPosition(positionText.StartIndex, positionText.EndIndex);
            var textWidth = getTextWidth(positionText.Text);
            var position = new Vector2(canterPosition.X - textWidth / 2, canterPosition.Y);
            setCurrentPosition(position);

            foreach (var c in text)
            {
                if (!AddCharacter(c))
                    break;
            }
        }

        private void setCurrentPosition(Vector2 position)
        {
            var prop = typeof(TextBuilder).GetField("currentPos", BindingFlags.Instance | BindingFlags.NonPublic);
            if (prop == null)
                throw new NullReferenceException();

            prop.SetValue(this, position);
        }

        private Vector2 getCenterPosition(int startCharIndex, int endCharIndex)
        {
            
            var startCharacter = Characters[startCharIndex];
            var endCharacter = Characters[endCharIndex - 1];

            // todo : should deal with multi-line issue.
            // todo : should deal with ruby/romaji case, or pass in outside?
            var x = (startCharacter.DrawRectangle.Left + endCharacter.DrawRectangle.Right) / 2;
            var y = startCharacter.DrawRectangle.Top;
            return new Vector2(x, y);
        }

        private float getTextWidth(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            return text.Sum(c => (getTexturedGlyph(c)?.Width ?? 0) * font.Size) + spacing.X * text.Length - 1;
        }

        private ITexturedCharacterGlyph getTexturedGlyph(char character)
        {
            return store.Get(font.FontName, character)
                   ?? store.Get(null, character)
                   ?? store.Get(font.FontName, fallbackCharacter)
                   ?? store.Get(null, fallbackCharacter);
        }
    }
}
