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
                    TopTexts = TestCaseTagHelper.ParsePositionTexts(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け", "[6]:－", "[4]:" }),
                    Romajies = TestCaseTagHelper.ParsePositionTexts(new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke", "[4,5]:yo", "[4]:" }),
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

    [TestCase("[0]:か", true)]
    [TestCase("[1]:ら", true)]
    [TestCase("[2]:お", true)]
    [TestCase("[3]:け", true)]
    [TestCase("[6]:－", true)]
    [TestCase("[4]:", true)] // Should be able to get the ruby/romaji text position even if text is empty.
    [TestCase("[-1,0]:か", true)] // will be fixed into "[0]:か"
    [TestCase("[0,1]:か", false)] // index is wrong.
    [TestCase("[0]:?", false)]
    public void TestGetRubyTagDrawRectangle(string rubyTag, bool valid)
    {
        prepareTestCase(() => lyricSpriteText.GetTopPositionTextDrawRectangle(TestCaseTagHelper.ParsePositionText(rubyTag)), valid);
    }

    [TestCase("[0]:ka", true)]
    [TestCase("[1]:ra", true)]
    [TestCase("[2]:o", true)]
    [TestCase("[3]:ke", true)]
    [TestCase("[4,5]:yo", true)]
    [TestCase("[4]:", true)] // Should be able to get the ruby/romaji text position even if text is empty.
    [TestCase("[-1,0]:ka", true)] // will be fixed into "[0]:ka"
    [TestCase("[0,1]:ka", false)]
    [TestCase("[0]:?", false)]
    public void TestGetRomajiTagDrawRectangle(string romajiTag, bool valid)
    {
        prepareTestCase(() => lyricSpriteText.GetRomajiTagDrawRectangle(TestCaseTagHelper.ParsePositionText(romajiTag)), valid);
    }

    private void prepareTestCase(Func<RectangleF?> func, bool valid)
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

        void moveTheBox(RectangleF? rectangleF)
        {
            if (rectangleF == null)
            {
                showSizeBox.Hide();
            }
            else
            {
                showSizeBox.Show();
                showSizeBox.Position = rectangleF.Value.TopLeft;
                showSizeBox.Size = rectangleF.Value.Size;
            }
        }
    }
}
