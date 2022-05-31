// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Sprites
{
    public class TestSceneKaraokeSpriteText : TestScene
    {
        private readonly TestKaraokeSpriteText karaokeSpriteText;
        private readonly SpriteText transformAmountSpriteText;

        private int transformAmount;

        [Resolved]
        private ShaderManager shaderManager { get; set; }

        public TestSceneKaraokeSpriteText()
        {
            Children = new Drawable[]
            {
                karaokeSpriteText = new TestKaraokeSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "カラオケ！",
                    Rubies = TestCaseTagHelper.ParsePositionTexts(new[] { "[0,1]:か", "[2,3]:お" }),
                    Romajies = TestCaseTagHelper.ParsePositionTexts(new[] { "[1,2]:ra", "[3,4]:ke" }),
                    LeftTextColour = Color4.Green,
                    RightTextColour = Color4.Red,
                    Scale = new Vector2(2),
                    TransformAction = () =>
                    {
                        transformAmount++;
                        updateTransformerCountText();
                    }
                },
                transformAmountSpriteText = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Y = 100
                }
            };

            updateTransformerCountText();

            void updateTransformerCountText()
            {
                transformAmountSpriteText.Text = $"Transform has been triggered {transformAmount} times";
                transformAmountSpriteText.FadeColour(Color4.Red, 100).Then().FadeColour(Color4.White);
            }
        }

        [TestCase("No", new string[] { })] // No time-tags.
        [TestCase("Normal", new[] { "[0,start]:500", "[1,start]:600", "[2,start]:1000", "[3,start]:1500", "[4,start]:2000" })] // Normal time-tag.
        [TestCase("Normal 2", new[] { "[0,start]:0", "[0,end]:100", "[1,start]:1000", "[1,end]:1100", "[2,start]:2000", "[2,end]:2100", "[3,start]:3000", "[3,end]:3100", "[4,start]:4000", "[4,end]:4100" })]
        [TestCase("Out of range", new[] { "[-1,start]:0", "[0,start]:500", "[1,end]:600", "[2,start]:1000", "[3,end]:1500", "[4,end]:2000", "[8,end]:2500" })] // Out-of-range time-tag, but it's acceptable now.
        [TestCase("Reverse", new[] { "[4,start]:2000", "[3,start]:1500", "[2,start]:1000", "[1,start]:600", "[0,start]:500" })] // Reverse order.
        [TestCase("Reverse index", new[] { "[0,start]:2000", "[1,start]:1500", "[2,start]:1000", "[3,start]:600", "[4,start]:500" })] // Normal time-tag with reverse time(will have reverse effect).
        [TestCase("Reverse time", new[] { "[4,start]:500", "[3,start]:600", "[2,start]:1000", "[1,start]:1500", "[0,start]:2000" })] // Reverse time-tag with non-reverse time(will have reverse effect).
        [TestCase("One", new[] { "[0,start]:500" })] // Only one time-tag.
        public void TestKaraokeSpriteTextTimeTags(string name, string[] timeTags)
        {
            AddStep($"Apply \"{name}\" time-tags", () =>
            {
                var startTime = Clock.CurrentTime;

                karaokeSpriteText.TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
                                                              .ToDictionary(k => k.Key + startTime, v => v.Value);
            });
        }

        [TestCase("カラオケー")]
        public void TestText(string text)
        {
            AddStep("Change text", () =>
            {
                karaokeSpriteText.Text = text;
            });
        }

        [TestCase(new[] { "[0,1]:123aaa", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }, true)]
        public void TestRuby(string[] rubyTags, bool boo)
        {
            AddStep("Change ruby", () =>
            {
                var ruby = TestCaseTagHelper.ParsePositionTexts(rubyTags);
                karaokeSpriteText.Rubies = ruby;
            });
        }

        [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }, true)]
        public void TestRomaji(string[] romajiTags, bool boo)
        {
            AddStep("Change romaji", () =>
            {
                var romajies = TestCaseTagHelper.ParsePositionTexts(romajiTags);
                karaokeSpriteText.Romajies = romajies;
            });
        }

        [TestCase(48, 24, 24)]
        [TestCase(48, 10, 24)]
        [TestCase(48, 24, 10)]
        public void TestFont(int mainFontSize, int rubyFontSize, int romajiFontSize)
        {
            AddStep("Change font", () =>
            {
                karaokeSpriteText.Font = new FontUsage(null, mainFontSize);
                karaokeSpriteText.RubyFont = new FontUsage(null, rubyFontSize);
                karaokeSpriteText.RomajiFont = new FontUsage(null, romajiFontSize);
            });
        }

        [Test]
        public void TestLeftTextColour()
        {
            AddStep("Change left text colour", () =>
            {
                // note: usually we use shader to adjust the style.
                karaokeSpriteText.LeftTextColour = Color4.Orange;
            });
        }

        [Test]
        public void TestRightTextColour()
        {
            AddStep("Change right text colour", () =>
            {
                karaokeSpriteText.RightTextColour = Color4.Blue;
            });
        }

        [TestCase(LyricTextAlignment.Auto, LyricTextAlignment.Auto)]
        [TestCase(LyricTextAlignment.Auto, LyricTextAlignment.Center)]
        [TestCase(LyricTextAlignment.Auto, LyricTextAlignment.EqualSpace)]
        [TestCase(LyricTextAlignment.Center, LyricTextAlignment.Auto)]
        [TestCase(LyricTextAlignment.Center, LyricTextAlignment.Center)]
        [TestCase(LyricTextAlignment.Center, LyricTextAlignment.EqualSpace)]
        [TestCase(LyricTextAlignment.EqualSpace, LyricTextAlignment.Auto)]
        [TestCase(LyricTextAlignment.EqualSpace, LyricTextAlignment.Center)]
        [TestCase(LyricTextAlignment.EqualSpace, LyricTextAlignment.EqualSpace)]
        public void TestAlignment(LyricTextAlignment rubyAlignment, LyricTextAlignment romajiAlignment)
        {
            AddStep("Change alignment", () =>
            {
                karaokeSpriteText.RomajiAlignment = rubyAlignment;
                karaokeSpriteText.RomajiAlignment = romajiAlignment;
            });
        }

        [TestCase(null, null, null)]
        [TestCase("(10,0)", null, null)]
        [TestCase(null, "(10,0)", null)]
        [TestCase(null, null, "(10,0)")]
        public void TestSpacing(string spacing, string rubySpacing, string romajiSpacing)
        {
            AddStep("Change spacing", () =>
            {
                karaokeSpriteText.Spacing = TestCaseVectorHelper.ParseVector2(spacing);
                karaokeSpriteText.RubySpacing = TestCaseVectorHelper.ParseVector2(rubySpacing);
                karaokeSpriteText.RomajiSpacing = TestCaseVectorHelper.ParseVector2(romajiSpacing);
            });
        }

        [TestCase(0, 0)]
        [TestCase(0, 20)]
        [TestCase(20, 0)]
        public void TestMarginPadding(int rubyMargin, int romajiMargin)
        {
            AddStep("Change margin", () =>
            {
                karaokeSpriteText.RubyMargin = rubyMargin;
                karaokeSpriteText.RomajiMargin = romajiMargin;
            });
        }

        [Test]
        public void TestLyricShaders()
        {
            AddStep("Apply the shader", () =>
            {
                karaokeSpriteText.LeftLyricTextShaders = new[]
                {
                    shaderManager.LocalInternalShader<OutlineShader>().With(s =>
                    {
                        s.Radius = 3;
                        s.Colour = Color4Extensions.FromHex("#FFDD77");
                        s.OutlineColour = Color4Extensions.FromHex("#CCA532");
                    })
                };
                karaokeSpriteText.RightLyricTextShaders = new[]
                {
                    shaderManager.LocalInternalShader<OutlineShader>().With(s =>
                    {
                        s.Radius = 3;
                        s.Colour = Color4Extensions.FromHex("#AA88FF");
                        s.OutlineColour = Color4Extensions.FromHex("#5932CC");
                    })
                };
            });

            AddStep("Clear shader", () =>
            {
                karaokeSpriteText.LeftLyricTextShaders = null;
                karaokeSpriteText.RightLyricTextShaders = null;
            });
        }

        private class TestKaraokeSpriteText : KaraokeSpriteText
        {
            public Action TransformAction;

            public override void RefreshStateTransforms()
            {
                base.RefreshStateTransforms();

                TransformAction?.Invoke();
            }
        }
    }
}
