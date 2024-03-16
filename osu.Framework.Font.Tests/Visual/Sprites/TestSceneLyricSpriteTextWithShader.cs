// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Sprites;

public partial class TestSceneLyricSpriteTextWithShader : BackgroundGridTestScene
{
    private readonly LyricSpriteText lyricSpriteText;

    public TestSceneLyricSpriteTextWithShader()
    {
        Child = lyricSpriteText = new LyricSpriteText
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Text = "カラオケ",
            TopTexts = TestCaseTagHelper.ParsePositionTexts(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }),
            BottomTexts = TestCaseTagHelper.ParsePositionTexts(new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke" }),
            Scale = new Vector2(2)
        };
    }

    [Test]
    public void ApplyShader()
    {
        AddStep("Apply static shader", () => lyricSpriteText.Shaders = new[]
        {
            GetShaderByType<OutlineShader>().With(s =>
            {
                s.Radius = 3;
                s.OutlineColour = Color4.Green;
            })
        });

        AddStep("Apply rainbow shader", () => lyricSpriteText.Shaders = new ICustomizedShader[]
        {
            GetShaderByType<OutlineShader>().With(s =>
            {
                s.Radius = 1;
                s.OutlineColour = Color4.Blue;
            }),
            new StepShader
            {
                Name = "Outline with rainbow effect",
                StepShaders = new ICustomizedShader[]
                {
                    GetShaderByType<OutlineShader>().With(s =>
                    {
                        s.Radius = 3;
                        s.OutlineColour = Color4.White;
                    }),
                    GetShaderByType<RainbowShader>()
                }
            },
        });

        AddStep("Remove all shader", () => lyricSpriteText.Shaders = Array.Empty<ICustomizedShader>());
    }
}
