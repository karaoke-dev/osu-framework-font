// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osuTK;

namespace osu.Framework.Graphics.Sprites;

public interface IHasTopText : IDrawable
{
    IReadOnlyList<PositionText> TopTexts { get; set; }

    FontUsage TopTextFont { get; set; }

    int TopTextMargin { get; set; }

    Vector2 TopTextSpacing { get; set; }

    LyricTextAlignment TopTextAlignment { get; set; }
}
