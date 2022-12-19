// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Sprites;

public partial class TestSceneKaraokeSpriteTextTransforms : BackgroundGridTestScene
{
    private const string left_text_color = "#FFDD77";
    private const string left_outline_color = "#CCA532";
    private const string left_shadow_color = "#6B5B2D";

    private const string right_text_color = "#AA88FF";
    private const string right_outline_color = "#5932CC";
    private const string right_shadow_color = "#3D2D6B";

    private const int outline_radius = 3;
    private const int shadow_sizing = 3;

    private const double start_time = 1000;
    private const double end_time = 5000;
    private const double extra_time = 500;

    private readonly ManualClock manualClock = new();
    private readonly TestKaraokeSpriteText karaokeSpriteText;

    public TestSceneKaraokeSpriteTextTransforms()
    {
        Child = karaokeSpriteText = new TestKaraokeSpriteText
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            LeftTextColour = Color4Extensions.FromHex(left_text_color),
            RightTextColour = Color4Extensions.FromHex(right_text_color),
            Clock = new FramedClock(manualClock),
            Scale = new Vector2(2),
        };

        AddLabel("Load lyric");

        AddStep("Single character", () =>
        {
            karaokeSpriteText.Text = "カ";
            karaokeSpriteText.TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { $"[0,start]:{start_time}", $"[0,end]:{end_time}" });
            karaokeSpriteText.Font = FontUsage.Default.With(size: 120);
        });

        AddStep("Multiple character", () =>
        {
            karaokeSpriteText.Text = "カラオケ";
            karaokeSpriteText.TimeTags = TestCaseTagHelper.ParseTimeTags(new[] { $"[0,start]:{start_time}", "[1,start]:2000", "[2,start]:3000", "[3,start]:4000", $"[3,end]:{end_time}" });
            karaokeSpriteText.Font = FontUsage.Default.With(size: 60);
        });

        AddLabel("Timing");

        AddSliderStep("Adjust clock time", start_time - extra_time, end_time + extra_time, start_time, time =>
        {
            manualClock.CurrentTime = time;
        });

        AddStep("Move to start time", () =>
        {
            manualClock.CurrentTime = start_time;
        });

        AddStep("Move to end time", () =>
        {
            manualClock.CurrentTime = end_time;
        });
    }

    [Test]
    public void TestNoneShader()
    {
        AddStep("Clear shader", () =>
        {
            karaokeSpriteText.LeftTextColour = Color4Extensions.FromHex(left_text_color);
            karaokeSpriteText.RightTextColour = Color4Extensions.FromHex(right_text_color);
            karaokeSpriteText.LeftLyricTextShaders = Array.Empty<ICustomizedShader>();
            karaokeSpriteText.RightLyricTextShaders = Array.Empty<ICustomizedShader>();
        });
    }

    [TestCase(false)]
    [TestCase(true)]
    public void TestApplyOutlineShader(bool differentSizing)
    {
        AddStep(getApplyDescription(differentSizing), () =>
        {
            karaokeSpriteText.LeftTextColour = Color4.White;
            karaokeSpriteText.RightTextColour = Color4.White;
            karaokeSpriteText.LeftLyricTextShaders = new[]
            {
                GetShaderByType<OutlineShader>().With(s =>
                {
                    s.Radius = outline_radius;
                    s.Colour = Color4Extensions.FromHex(left_text_color);
                    s.OutlineColour = Color4Extensions.FromHex(left_outline_color);
                })
            };
            karaokeSpriteText.RightLyricTextShaders = new[]
            {
                GetShaderByType<OutlineShader>().With(s =>
                {
                    s.Radius = differentSizing ? outline_radius * 2 : outline_radius;
                    s.Colour = Color4Extensions.FromHex(right_text_color);
                    s.OutlineColour = Color4Extensions.FromHex(right_outline_color);
                })
            };
        });
    }

    [TestCase(false)]
    [TestCase(true)]
    public void TestApplyShadowShader(bool differentSizing)
    {
        AddStep(getApplyDescription(differentSizing), () =>
        {
            karaokeSpriteText.LeftTextColour = Color4Extensions.FromHex(left_text_color);
            karaokeSpriteText.RightTextColour = Color4Extensions.FromHex(right_text_color);
            karaokeSpriteText.LeftLyricTextShaders = new[]
            {
                GetShaderByType<ShadowShader>().With(s =>
                {
                    s.ShadowOffset = new Vector2(shadow_sizing);
                    s.ShadowColour = Color4Extensions.FromHex(left_shadow_color);
                })
            };
            karaokeSpriteText.RightLyricTextShaders = new[]
            {
                GetShaderByType<ShadowShader>().With(s =>
                {
                    s.ShadowOffset = new Vector2(differentSizing ? shadow_sizing * 2 : shadow_sizing);
                    s.ShadowColour = Color4Extensions.FromHex(right_shadow_color);
                })
            };
        });
    }

    [TestCase(false)]
    [TestCase(true)]
    public void TestApplyOutlineAndShadowShader(bool differentSizing)
    {
        AddStep(getApplyDescription(differentSizing), () =>
        {
            karaokeSpriteText.LeftTextColour = Color4.White;
            karaokeSpriteText.RightTextColour = Color4.White;
            karaokeSpriteText.LeftLyricTextShaders = new[]
            {
                new StepShader
                {
                    StepShaders = new ICustomizedShader[]
                    {
                        GetShaderByType<OutlineShader>().With(s =>
                        {
                            s.Radius = outline_radius;
                            s.Colour = Color4Extensions.FromHex(left_text_color);
                            s.OutlineColour = Color4Extensions.FromHex(left_outline_color);
                        }),
                        GetShaderByType<ShadowShader>().With(s =>
                        {
                            s.ShadowOffset = new Vector2(shadow_sizing);
                            s.ShadowColour = Color4Extensions.FromHex(left_shadow_color);
                        })
                    }
                }
            };
            karaokeSpriteText.RightLyricTextShaders = new[]
            {
                new StepShader
                {
                    StepShaders = new ICustomizedShader[]
                    {
                        GetShaderByType<OutlineShader>().With(s =>
                        {
                            s.Radius = differentSizing ? outline_radius * 2 : outline_radius;
                            s.Colour = Color4Extensions.FromHex(right_text_color);
                            s.OutlineColour = Color4Extensions.FromHex(right_outline_color);
                        }),
                        GetShaderByType<ShadowShader>().With(s =>
                        {
                            s.ShadowOffset = new Vector2(differentSizing ? shadow_sizing * 2 : shadow_sizing);
                            s.ShadowColour = Color4Extensions.FromHex(right_shadow_color);
                        })
                    }
                }
            };
        });
    }

    private static string getApplyDescription(bool applyDifferentSizing)
        => applyDifferentSizing ? "Apply shader with different sizing" : "Apply shader";

    private class TestKaraokeSpriteText : KaraokeSpriteText
    {
        public override bool RemoveCompletedTransforms => false;
    }
}
