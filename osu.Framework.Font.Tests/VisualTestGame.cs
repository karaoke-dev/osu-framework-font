﻿// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace osu.Framework.Font.Tests;

internal partial class VisualTestGame : TestGame
{
    [BackgroundDependencyLoader]
    private void load()
    {
        Child = new DrawSizePreservingFillContainer
        {
            Children = new Drawable[]
            {
                new TestBrowser(),
                new CursorContainer(),
            },
        };
    }

    public override void SetHost(GameHost host)
    {
        base.SetHost(host);
        host.Window.CursorState |= CursorState.Hidden;
    }
}
