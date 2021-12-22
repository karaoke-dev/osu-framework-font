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
        private readonly FontUsage mainTextFont;
        private readonly FontUsage font;
        private readonly Vector2 startOffset;
        private readonly Vector2 spacing;

        private readonly RelativePosition relativePosition;
        private readonly LyricTextAlignment alignment;

        /// <summary>
        /// Creates a new <see cref="TextBuilder"/>.
        /// </summary>
        /// <param name="store">The store from which glyphs are to be retrieved from.</param>
        /// <param name="mainTextFont">The main text's font.<paramref name="store"/>.</param>
        /// <param name="font">The font to use for glyph lookups from <paramref name="store"/>.</param>
        /// <param name="useFontSizeAsHeight">True to use the provided <see cref="font"/> size as the height for each line. False if the height of each individual glyph should be used.</param>
        /// <param name="startOffset">The offset at which characters should begin being added at.</param>
        /// <param name="spacing">The spacing between characters.</param>
        /// <param name="maxWidth">The maximum width of the resulting text bounds.</param>
        /// <param name="characterList">That list to contain all resulting <see cref="TextBuilderGlyph"/>s.</param>
        /// <param name="neverFixedWidthCharacters">The characters for which fixed width should never be applied.</param>
        /// <param name="fallbackCharacter">The character to use if a glyph lookup fails.</param>
        /// <param name="fixedWidthReferenceCharacter">The character to use to calculate the fixed width width. Defaults to 'm'.</param>
        /// <param name="relativePosition">Should be added into top or bottom.</param>
        /// <param name="alignment">Lyric text alignment.</param>
        public PositionTextBuilder(ITexturedGlyphLookupStore store, FontUsage mainTextFont, FontUsage font, float maxWidth = int.MaxValue, bool useFontSizeAsHeight = true, Vector2 startOffset = default,
                                   Vector2 spacing = default, List<TextBuilderGlyph> characterList = null, char[] neverFixedWidthCharacters = null,
                                   char fallbackCharacter = '?', char fixedWidthReferenceCharacter = 'm', RelativePosition relativePosition = RelativePosition.Top, LyricTextAlignment alignment = LyricTextAlignment.Auto)
            : base(store, font, maxWidth, useFontSizeAsHeight, startOffset, spacing, characterList, neverFixedWidthCharacters, fallbackCharacter, fixedWidthReferenceCharacter)
        {
            this.store = store;
            this.mainTextFont = mainTextFont;
            this.font = font;
            this.startOffset = startOffset;
            this.spacing = spacing;
            this.fallbackCharacter = fallbackCharacter;

            this.relativePosition = relativePosition;
            this.alignment = alignment;
        }

        /// <summary>
        /// Appends text to this <see cref="TextBuilder"/>.
        /// </summary>
        /// <param name="positionText">The text to append.</param>
        public void AddText(PositionText positionText)
        {
            var text = positionText.Text;
            if (string.IsNullOrEmpty(text))
                return;

            // calculate start render position
            var canterPosition = getCenterPosition(positionText.StartIndex, positionText.EndIndex);
            var textWidth = getSubTextWidth(text);
            var yOffset = -getCharHeight(text.FirstOrDefault(), font);
            var position = new Vector2(canterPosition.X - textWidth / 2, canterPosition.Y + yOffset);

            // set start render position
            setCurrentPosition(position + startOffset);

            // render the chars.
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
            var starCharacter = Characters[startCharIndex];
            var endCharacter = Characters[endCharIndex - 1];
            var startCharacterRectangle = starCharacter.DrawRectangle;
            var endCharacterRectangle = endCharacter.DrawRectangle;

            // if center position is between two lines, then should let canter position in the first line.
            var leftX = startCharacterRectangle.Left;
            var rightX = endCharacterRectangle.Right > leftX
                ? endCharacterRectangle.Right
                : Characters.Max(c => c.DrawRectangle.Right);
            var x = (leftX + rightX) / 2;

            // because each character has different height, so we need to get base text height from here.
            var y = startCharacterRectangle.Centre.Y - starCharacter.YOffset;

            // return center position.
            var yOffset = relativePosition == RelativePosition.Top ? 0 : getCharHeight(starCharacter.Character, mainTextFont);
            return new Vector2(x, y + yOffset);
        }

        private float getSubTextWidth(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            return text.Sum(c => getCharWidth(c, font)) + spacing.X * (text.Length - 1);
        }

        private float getCharWidth(char c, FontUsage fontUsage)
        {
            var texture = getTexturedGlyph(c);
            if (texture == null)
                return 0;

            return texture.Width * fontUsage.Size;
        }

        private float getCharHeight(char c, FontUsage fontUsage)
        {
            var texture = getTexturedGlyph(c);
            if (texture == null)
                return 0;

            return texture.Baseline * fontUsage.Size;
        }

        private ITexturedCharacterGlyph getTexturedGlyph(char character)
        {
            return store.Get(font.FontName, character)
                   ?? store.Get(null, character)
                   ?? store.Get(font.FontName, fallbackCharacter)
                   ?? store.Get(null, fallbackCharacter);
        }
    }

    public enum RelativePosition
    {
        Top,

        Bottom
    }
}
