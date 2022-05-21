// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Text;

namespace osu.Framework.Utils
{
    public class TextBuilderGlyphUtils
    {
        public static RectangleF GetCharacterRectangle(TextBuilderGlyph character, bool drawSizeOnly)
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

        public static RectangleF GetCharacterRectangle(PositionTextBuilderGlyph character, bool drawSizeOnly)
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
    }
}
