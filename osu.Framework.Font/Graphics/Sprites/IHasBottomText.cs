// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osuTK;

namespace osu.Framework.Graphics.Sprites;

public interface IHasBottomText : IDrawable
{
    IReadOnlyList<PositionText> BottomTexts { get; set; }

    FontUsage BottomTextFont { get; set; }

    int BottomTextMargin { get; set; }

    Vector2 BottomTextSpacing { get; set; }

    LyricTextAlignment BottomTextAlignment { get; set; }
}
