// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osuTK;

namespace osu.Framework.Graphics.Sprites;

public interface IHasRomaji : IDrawable
{
    IReadOnlyList<PositionText> Romajies { get; set; }

    FontUsage RomajiFont { get; set; }

    int RomajiMargin { get; set; }

    Vector2 RomajiSpacing { get; set; }

    LyricTextAlignment RomajiAlignment { get; set; }
}
