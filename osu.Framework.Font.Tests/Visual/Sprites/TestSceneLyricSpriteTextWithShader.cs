// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Sprites
{
    public class TestSceneLyricSpriteTextWithShader : BackgroundGridTestSample
    {
        private readonly LyricSpriteText lyricSpriteText;

        public TestSceneLyricSpriteTextWithShader()
        {
            Child = lyricSpriteText = new LyricSpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "カラオケ",
                Rubies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }),
                Romajies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }),
                Scale = new Vector2(2)
            };
        }

        [Test]
        public void ApplyShader()
        {
            var outlineShader = GetShader(OutlineShader.SHADER_NAME);
            var rainbowShader = GetShader(RainbowShader.SHADER_NAME);

            AddStep("Apply static shader", () => lyricSpriteText.Shaders = new[]
            {
                new OutlineShader(outlineShader)
                {
                    Radius = 10,
                    OutlineColour = Color4.Green,
                },
            });

            AddStep("Apply rainbow shader", () => lyricSpriteText.Shaders = new IShader[]
            {
                new OutlineShader(outlineShader)
                {
                    Radius = 1,
                    OutlineColour = Color4.Blue,
                },
                new StepShader
                {
                    Name = "Outline with rainbow effect",
                    StepShaders = new IShader[]
                    {
                        new OutlineShader(outlineShader)
                        {
                            Radius = 10,
                            OutlineColour = Color4.White,
                        },
                        new RainbowShader(rainbowShader)
                    }
                },
            });

            AddStep("Remove all shader", () => lyricSpriteText.Shaders = null);
        }
    }
}
