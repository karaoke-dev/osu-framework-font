// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Testing;

namespace osu.Framework.Font.Tests;

internal partial class AutomatedVisualTestGame : TestGame
{
    public AutomatedVisualTestGame()
    {
        Add(new TestBrowserTestRunner(new TestBrowser()));
    }
}
