// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Sprites
{
    public class TestSceneKaraokeSpriteTextWithShader : BackgroundGridTestScene
    {
        private readonly ManualClock manualClock = new();

        private readonly TestKaraokeSpriteText karaokeSpriteText;

        public TestSceneKaraokeSpriteTextWithShader()
        {
            Child = karaokeSpriteText = new TestKaraokeSpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "カラオケ！",
                Rubies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }),
                Romajies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }),
                TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { "[0,start]:500", "[1,start]:600", "[2,start]:1000", "[3,start]:1500", "[4,start]:2000" }),
                Scale = new Vector2(2),
                LeftTextColour = Color4.Green,
                RightTextColour = Color4.Red,
                Clock = new FramedClock(manualClock),
            };

            AddSliderStep("Adjust clock time", 0, 3000, 1000, time =>
            {
                manualClock.CurrentTime = time;
            });
        }

        [Test]
        public void ApplyShader()
        {
            AddStep("Apply CRT shader", () => karaokeSpriteText.Shaders = new[]
            {
                GetShader("CRT"),
            });

            AddStep("Clear shader", () => karaokeSpriteText.Shaders = null);
        }

        [Test]
        public void ApplyLeftLyricTextShader()
        {
            AddStep("Apply Outline shader in left text", () =>
            {
                karaokeSpriteText.LeftTextColour = Color4.White;
                karaokeSpriteText.LeftLyricTextShaders = new[]
                {
                    GetShaderByType<OutlineShader>().With(s =>
                    {
                        s.Radius = 5;
                        s.OutlineColour = Color4.Green;
                    })
                };
            });

            AddStep("Clear shader from left text", () =>
            {
                karaokeSpriteText.LeftTextColour = Color4.Green;
                karaokeSpriteText.LeftLyricTextShaders = null;
            });
        }

        [Test]
        public void ApplyRightLyricTextShader()
        {
            AddStep("Apply Outline shader in right text", () =>
            {
                karaokeSpriteText.RightTextColour = Color4.White;
                karaokeSpriteText.RightLyricTextShaders = new IShader[]
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
                                s.Radius = 10;
                                s.OutlineColour = Color4.White;
                            }),
                            GetShaderByType<RainbowShader>()
                        }
                    },
                };
            });

            AddStep("Clear shader from right text", () =>
            {
                karaokeSpriteText.RightTextColour = Color4.Red;
                karaokeSpriteText.RightLyricTextShaders = null;
            });
        }

        private class TestKaraokeSpriteText : KaraokeSpriteText
        {
            public override bool RemoveCompletedTransforms => false;
        }
    }
}
