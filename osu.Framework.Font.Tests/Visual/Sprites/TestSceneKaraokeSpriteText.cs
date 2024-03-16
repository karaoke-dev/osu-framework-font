// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Sprites;

public partial class TestSceneKaraokeSpriteText : FrameworkTestScene
{
    private readonly TestKaraokeSpriteText karaokeSpriteText;
    private readonly SpriteText transformAmountSpriteText;

    private int transformAmount;

    [Resolved]
    private ShaderManager shaderManager { get; set; } = null!;

    public TestSceneKaraokeSpriteText()
    {
        Children = new Drawable[]
        {
            karaokeSpriteText = new TestKaraokeSpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Text = "カラオケ！",
                TopTexts = TestCaseTagHelper.ParsePositionTexts(new[] { "[0]:か", "[2]:お" }),
                BottomTexts = TestCaseTagHelper.ParsePositionTexts(new[] { "[1]:ra", "[3]:ke" }),
                LeftTextColour = Color4.Green,
                RightTextColour = Color4.Red,
                Scale = new Vector2(2),
                TransformAction = () =>
                {
                    transformAmount++;
                    updateTransformerCountText();
                },
            },
            transformAmountSpriteText = new SpriteText
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Y = 100,
            },
        };

        updateTransformerCountText();

        void updateTransformerCountText()
        {
            Debug.Assert(transformAmountSpriteText != null);
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

    [TestCase(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }, true)]
    public void TestTopText(string[] topTexts, bool boo)
    {
        AddStep("Change top text", () =>
        {
            var positionTexts = TestCaseTagHelper.ParsePositionTexts(topTexts);
            karaokeSpriteText.TopTexts = positionTexts;
        });
    }

    [TestCase(new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke" }, true)]
    public void TestBottomText(string[] bottomTexts, bool boo)
    {
        AddStep("Change bottom text", () =>
        {
            var positionTexts = TestCaseTagHelper.ParsePositionTexts(bottomTexts);
            karaokeSpriteText.BottomTexts = positionTexts;
        });
    }

    [TestCase(48, 24, 24)]
    [TestCase(48, 10, 24)]
    [TestCase(48, 24, 10)]
    public void TestFont(int mainFontSize, int topTextFontSize, int bottomTextFontSize)
    {
        AddStep("Change font", () =>
        {
            karaokeSpriteText.Font = new FontUsage(null, mainFontSize);
            karaokeSpriteText.TopTextFont = new FontUsage(null, topTextFontSize);
            karaokeSpriteText.BottomTextFont = new FontUsage(null, bottomTextFontSize);
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
    public void TestAlignment(LyricTextAlignment topTextAlignment, LyricTextAlignment bottomTextAlignment)
    {
        AddStep("Change alignment", () =>
        {
            karaokeSpriteText.TopTextAlignment = topTextAlignment;
            karaokeSpriteText.BottomTextAlignment = bottomTextAlignment;
        });
    }

    [TestCase(null, null, null)]
    [TestCase("(10,0)", null, null)]
    [TestCase(null, "(10,0)", null)]
    [TestCase(null, null, "(10,0)")]
    public void TestSpacing(string spacing, string topTextSpacing, string bottomTextSpacing)
    {
        AddStep("Change spacing", () =>
        {
            karaokeSpriteText.Spacing = TestCaseVectorHelper.ParseVector2(spacing);
            karaokeSpriteText.TopTextSpacing = TestCaseVectorHelper.ParseVector2(topTextSpacing);
            karaokeSpriteText.BottomTextSpacing = TestCaseVectorHelper.ParseVector2(bottomTextSpacing);
        });
    }

    [TestCase(0, 0)]
    [TestCase(0, 20)]
    [TestCase(20, 0)]
    public void TestMarginPadding(int topTextMargin, int bottomTextMargin)
    {
        AddStep("Change margin", () =>
        {
            karaokeSpriteText.TopTextMargin = topTextMargin;
            karaokeSpriteText.BottomTextMargin = bottomTextMargin;
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
                }),
            };
            karaokeSpriteText.RightLyricTextShaders = new[]
            {
                shaderManager.LocalInternalShader<OutlineShader>().With(s =>
                {
                    s.Radius = 3;
                    s.Colour = Color4Extensions.FromHex("#AA88FF");
                    s.OutlineColour = Color4Extensions.FromHex("#5932CC");
                }),
            };
        });

        AddStep("Clear shader", () =>
        {
            karaokeSpriteText.LeftLyricTextShaders = Array.Empty<ICustomizedShader>();
            karaokeSpriteText.RightLyricTextShaders = Array.Empty<ICustomizedShader>();
        });
    }

    private partial class TestKaraokeSpriteText : KaraokeSpriteText
    {
        public Action? TransformAction;

        public override void RefreshStateTransforms()
        {
            base.RefreshStateTransforms();

            TransformAction?.Invoke();
        }
    }
}
