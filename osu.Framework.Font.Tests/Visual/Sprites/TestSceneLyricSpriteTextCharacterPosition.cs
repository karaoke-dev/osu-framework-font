// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Sprites;

public partial class TestSceneLyricSpriteTextCharacterPosition : BackgroundGridTestScene
{
    private readonly Box showSizeBox;
    private readonly LyricSpriteText lyricSpriteText;

    public TestSceneLyricSpriteTextCharacterPosition()
    {
        Child = new Container
        {
            AutoSizeAxes = Axes.Both,
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Scale = new Vector2(2),
            Children = new Drawable[]
            {
                showSizeBox = new Box
                {
                    Colour = Color4.Green
                },
                lyricSpriteText = new LyricSpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "カラオケyo－",
                    Rubies = TestCaseTagHelper.ParsePositionTexts(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け", "[4,5]:－", "[4,5]:" }),
                    Romajies = TestCaseTagHelper.ParsePositionTexts(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke", "[4,5]:yo", "[4,5]:" }),
                },
            }
        };

        AddLabel("Shader");

        AddStep("Clear shader", () =>
        {
            lyricSpriteText.Shaders = Array.Empty<ICustomizedShader>();
        });

        AddStep("Apply single shader", () =>
        {
            lyricSpriteText.Shaders = new[]
            {
                GetShaderByType<OutlineShader>().With(s =>
                {
                    s.Radius = 3;
                    s.OutlineColour = Color4.Green;
                })
            };
        });

        AddStep("Apply multiple shaders", () =>
        {
            lyricSpriteText.Shaders = new[]
            {
                new StepShader
                {
                    StepShaders = new ICustomizedShader[]
                    {
                        GetShaderByType<OutlineShader>().With(s =>
                        {
                            s.Radius = 3;
                            s.OutlineColour = Color4.Blue;
                        }),
                        GetShaderByType<ShadowShader>().With(s =>
                        {
                            s.ShadowOffset = new Vector2(2);
                            s.ShadowColour = Color4.Aqua;
                        })
                    }
                }
            };
        });
    }

    [TestCase(0, TextIndex.IndexState.Start, true)]
    [TestCase(6, TextIndex.IndexState.End, true)]
    [TestCase(-1, TextIndex.IndexState.End, false)]
    [TestCase(7, TextIndex.IndexState.Start, false)]
    public void TestGetTextIndexXPosition(int index, TextIndex.IndexState state, bool valid)
    {
        prepareTestCase(() =>
        {
            var position = lyricSpriteText.GetTextIndexXPosition(new TextIndex(index, state));
            return new RectangleF(position, 20, 1, 64);
        }, valid);
    }

    [TestCase(0, true)]
    [TestCase(6, true)]
    [TestCase(-1, false)]
    [TestCase(7, false)]
    public void TestGetCharacterDrawRectangle(int index, bool valid)
    {
        prepareTestCase(() => lyricSpriteText.GetCharacterDrawRectangle(index), valid);
    }

    [TestCase("[0,1]:か", true)]
    [TestCase("[1,2]:ら", true)]
    [TestCase("[2,3]:お", true)]
    [TestCase("[3,4]:け", true)]
    [TestCase("[4,5]:－", true)]
    [TestCase("[4,5]:", true)] // Should be able to get the ruby/romaji text position even if text is empty.
    [TestCase("[-1,1]:か", true)] // will be fixed into "[0,1]:か"
    [TestCase("[0,2]:か", false)]
    [TestCase("[0,1]:?", false)]
    public void TestGetRubyTagDrawRectangle(string rubyTag, bool valid)
    {
        prepareTestCase(() => lyricSpriteText.GetRubyTagDrawRectangle(TestCaseTagHelper.ParsePositionText(rubyTag)), valid);
    }

    [TestCase("[0,1]:ka", true)]
    [TestCase("[1,2]:ra", true)]
    [TestCase("[2,3]:o", true)]
    [TestCase("[3,4]:ke", true)]
    [TestCase("[4,5]:yo", true)]
    [TestCase("[4,5]:", true)] // Should be able to get the ruby/romaji text position even if text is empty.
    [TestCase("[-1,1]:ka", true)] // will be fixed into "[0,1]:ka"
    [TestCase("[0,2]:ka", false)]
    [TestCase("[0,1]:?", false)]
    public void TestGetRomajiTagDrawRectangle(string rubyTag, bool valid)
    {
        prepareTestCase(() => lyricSpriteText.GetRomajiTagDrawRectangle(TestCaseTagHelper.ParsePositionText(rubyTag)), valid);
    }

    private void prepareTestCase(Func<RectangleF> func, bool valid)
    {
        if (valid)
        {
            AddStep("Get the rectangle", () =>
            {
                var rectangle = func();
                moveTheBox(rectangle);
            });
        }
        else
        {
            AddStep("Oops", () =>
            {
                Assert.Catch<ArgumentOutOfRangeException>(() =>
                {
                    func();
                });
            });
        }

        void moveTheBox(RectangleF rectangleF)
        {
            showSizeBox.Position = rectangleF.TopLeft;
            showSizeBox.Size = rectangleF.Size;
        }
    }
}
