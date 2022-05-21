// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Text;

namespace osu.Framework.Utils
{
    public static class TextBuilderGlyphUtils
    {
        private static float getCharacterTopOffset(ICharacterGlyph character)
            => character.Baseline * 0.3f;

        private static float getCharacterBottomOffset(ICharacterGlyph character)
            => character.Baseline * 0.03f;

        public static RectangleF GetCharacterRectangle(TextBuilderGlyph character, bool drawSizeOnly)
        {
            if (drawSizeOnly)
                return character.DrawRectangle;

            return character.DrawRectangle.Inflate(new MarginPadding
            {
                Top = character.YOffset - getCharacterTopOffset(character),
                Bottom = character.Baseline - character.Height - character.YOffset + getCharacterBottomOffset(character),
            });
        }

        public static RectangleF GetCharacterRectangle(PositionTextBuilderGlyph character, bool drawSizeOnly)
        {
            if (drawSizeOnly)
                return character.DrawRectangle;

            return character.DrawRectangle.Inflate(new MarginPadding
            {
                Top = character.YOffset - getCharacterTopOffset(character),
                Bottom = character.Baseline - character.Height - character.YOffset + getCharacterBottomOffset(character),
            });
        }
    }
}
