// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Reflection;
using osu.Framework.Allocation;
using osu.Framework.IO.Stores;

namespace osu.Framework.Font.Tests;

internal class TestGame : Game
{
    [BackgroundDependencyLoader]
    private void load()
    {
        // add store from osu.game.resources
        Resources.AddStore(new DllResourceStore(@"osu.Game.Resources.dll"));

        // add store from main project
        Resources.AddStore(new NamespacedResourceStore<byte[]>(new ShaderResourceStore(), "Resources"));

        // add store from test project
        Resources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore(Assembly.GetExecutingAssembly().Location), "Resources"));

        // Add font resource
        AddFont(Resources, @"Fonts/osuFont");

        AddFont(Resources, @"Fonts/Torus/Torus-Regular");
        AddFont(Resources, @"Fonts/Torus/Torus-Light");
        AddFont(Resources, @"Fonts/Torus/Torus-SemiBold");
        AddFont(Resources, @"Fonts/Torus/Torus-Bold");

        AddFont(Resources, @"Fonts/Inter/Inter-Regular");
        AddFont(Resources, @"Fonts/Inter/Inter-RegularItalic");
        AddFont(Resources, @"Fonts/Inter/Inter-Light");
        AddFont(Resources, @"Fonts/Inter/Inter-LightItalic");
        AddFont(Resources, @"Fonts/Inter/Inter-SemiBold");
        AddFont(Resources, @"Fonts/Inter/Inter-SemiBoldItalic");
        AddFont(Resources, @"Fonts/Inter/Inter-Bold");
        AddFont(Resources, @"Fonts/Inter/Inter-BoldItalic");

        AddFont(Resources, @"Fonts/Noto/Noto-Basic");
        AddFont(Resources, @"Fonts/Noto/Noto-Hangul");
        AddFont(Resources, @"Fonts/Noto/Noto-CJK-Basic");
        AddFont(Resources, @"Fonts/Noto/Noto-CJK-Compatibility");
        AddFont(Resources, @"Fonts/Noto/Noto-Thai");

        AddFont(Resources, @"Fonts/Venera/Venera-Light");
        AddFont(Resources, @"Fonts/Venera/Venera-Bold");
        AddFont(Resources, @"Fonts/Venera/Venera-Black");
    }
}
