// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Utils;
using osuTK;

namespace osu.Framework.Text;

public class PositionTextFormatter
{
    private readonly List<TextBuilderGlyph> characterList;
    private readonly RelativePosition relativePosition;
    private readonly LyricTextAlignment alignment;
    private readonly Vector2 spacing;
    private readonly int margin;

    public PositionTextFormatter(List<TextBuilderGlyph> characterList,
                                 RelativePosition relativePosition,
                                 LyricTextAlignment alignment = LyricTextAlignment.Auto,
                                 Vector2 spacing = new(),
                                 int margin = 0)
    {
        this.characterList = characterList;
        this.relativePosition = relativePosition;
        this.alignment = alignment;
        this.spacing = spacing;
        this.margin = margin;
    }

    public PositionTextBuilderGlyph[] Calculate(PositionText positionText, TextBuilderGlyph[] glyphs)
    {
        if (glyphs == null || glyphs.Length == 0)
            throw new ArgumentException($"{nameof(glyphs)} cannot be empty");

        // get some position related params.
        var mainCharacterRect = getMainCharacterRectangleF(positionText.StartIndex, positionText.EndIndex);
        var subTextSize = getPositionTextSize(glyphs);

        // calculate the start draw position.
        var startPosition = getPositionTextStartPosition(mainCharacterRect, subTextSize.X, glyphs.First().Baseline, relativePosition, margin);

        // then it's time to shift the position.
        return createPositionTextBuilderGlyphs(glyphs, startPosition);
    }

    private static PositionTextBuilderGlyph[] createPositionTextBuilderGlyphs(TextBuilderGlyph[] glyphs, Vector2 startPosition)
    {
        return glyphs.Select(x =>
        {
            var newPosition = startPosition + x.DrawRectangle.TopLeft;
            return new PositionTextBuilderGlyph(x, newPosition);
        }).ToArray();
    }

    private static Vector2 getPositionTextStartPosition(RectangleF mainTextRect, float subTextWidth, float baseLine, RelativePosition relativePosition, int margin)
    {
        var drawXPosition = mainTextRect.Centre.X - subTextWidth / 2;

        return relativePosition switch
        {
            RelativePosition.Top => new Vector2(drawXPosition, mainTextRect.Top - margin - baseLine),
            RelativePosition.Bottom => new Vector2(drawXPosition, mainTextRect.Bottom + margin),
            _ => throw new ArgumentOutOfRangeException(nameof(relativePosition), relativePosition, null),
        };
    }

    private static Vector2 getPositionTextSize(TextBuilderGlyph[] glyphs)
        => glyphs.Select(TextBuilderGlyphUtils.GetCharacterSizeRectangle)
                 .Aggregate(RectangleF.Union).Size;

    private RectangleF getMainCharacterRectangleF(int startCharIndex, int endCharIndex)
    {
        var starCharacter = characterList[startCharIndex];
        var endCharacter = characterList[endCharIndex];
        var startCharacterRectangle = TextBuilderGlyphUtils.GetCharacterSizeRectangle(starCharacter);
        var endCharacterRectangle = TextBuilderGlyphUtils.GetCharacterSizeRectangle(endCharacter);

        var position = startCharacterRectangle.TopLeft;

        // if center position is between two lines, then should let canter position in the first line.
        var leftX = startCharacterRectangle.Left;
        var rightX = endCharacterRectangle.Right > leftX
            ? endCharacterRectangle.Right
            : characterList.Max(c => c.DrawRectangle.Right);

        // because each character has different height, so we need to get base text height from here.
        var width = rightX - leftX;
        var height = Math.Max(startCharacterRectangle.Height, endCharacterRectangle.Height);

        // return center position.
        return new RectangleF(position, new Vector2(width, height));
    }
}

public enum RelativePosition
{
    Top,

    Bottom,
}
