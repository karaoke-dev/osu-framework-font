// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osuTK;

namespace osu.Framework.Graphics.Sprites
{
    public interface IHasRuby : IDrawable
    {
        IReadOnlyList<PositionText> Rubies { get; set; }

        FontUsage RubyFont { get; set; }

        int RubyMargin { get; set; }

        Vector2 RubySpacing { get; set; }

        LyricTextAlignment RubyAlignment { get; set; }
    }
}
