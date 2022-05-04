// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Sprites
{
    public class TestSceneLyricSpriteTextWithShader : BackgroundGridTestScene
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
            AddStep("Apply static shader", () => lyricSpriteText.Shaders = new[]
            {
                GetShaderByType<OutlineShader>().With(s =>
                {
                    s.Radius = 3;
                    s.OutlineColour = Color4.Green;
                })
            });

            AddStep("Apply rainbow shader", () => lyricSpriteText.Shaders = new IShader[]
            {
                // comment the shader out until lyric sprite text support multiple shader.
                // GetShaderByType<OutlineShader>().With(s =>
                // {
                //     s.Radius = 1;
                //     s.OutlineColour = Color4.Blue;
                // }),
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

            AddStep("Remove all shader", () => lyricSpriteText.Shaders = null);
        }
    }
}
