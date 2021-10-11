// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Sprites
{
    public class TestSceneKaraokeSpriteTextWithShader : BackgroundGridTestSample
    {
        private readonly KaraokeSpriteText karaokeSpriteText;

        public TestSceneKaraokeSpriteTextWithShader()
        {
            Child = karaokeSpriteText = new KaraokeSpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "カラオケ！",
                Rubies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }),
                Romajies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }),
                Scale = new Vector2(2),
                LeftTextColour = Color4.Green,
                RightTextColour = Color4.Red,
            };
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
                    GetShaderByType<OutlineShader>().With(s =>
                    {
                        s.Radius = 1;
                        s.OutlineColour = Color4.Blue;
                    }),
                    new StepShader
                    {
                        Name = "Outline with rainbow effect",
                        StepShaders = new IShader[]
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

        protected new void AddStep(string description, Action action)
        {
            base.AddStep(description, () =>
            {
                action?.Invoke();
                resetTime();
            });

            void resetTime()
            {
                var startTime = Time.Current;
                karaokeSpriteText.TimeTags = new Dictionary<TextIndex, double>
                {
                    { new TextIndex(0), startTime + 500 },
                    { new TextIndex(1), startTime + 600 },
                    { new TextIndex(2), startTime + 1000 },
                    { new TextIndex(3), startTime + 1500 },
                    { new TextIndex(4), startTime + 2000 },
                };
            }
        }
    }
}
