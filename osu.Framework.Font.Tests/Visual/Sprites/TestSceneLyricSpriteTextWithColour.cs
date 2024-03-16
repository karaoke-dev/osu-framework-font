// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace osu.Framework.Font.Tests.Visual.Sprites;

public partial class TestSceneLyricSpriteTextWithColour : BackgroundGridTestScene
{
    public TestSceneLyricSpriteTextWithColour()
    {
        Child = new Container
        {
            RelativeSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                new LyricSpriteText
                {
                    Name = "Drawable with customized draw node.",
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "カラオケ",
                    TopTexts = TestCaseTagHelper.ParsePositionTexts(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }),
                    BottomTexts = TestCaseTagHelper.ParsePositionTexts(new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke" }),
                    Y = 60,
                },
                new BufferedContainer
                {
                    Name = "Drawable with internal draw node.",
                    RelativeSizeAxes = Axes.Both,
                    Child = new Box
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(30)
                    }
                },
                new SpriteText
                {
                    Name = "Drawable with internal draw node.",
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "カラオケ",
                    Scale = new Vector2(2),
                    Y = -60,
                }
            }
        };
    }

    [SetUp]
    public void Setup()
    {
        Child.Alpha = 1;
        Child.Colour = Colour4.White;
    }

    [TestCase(1)]
    [TestCase(0.7f)]
    [TestCase(0.3f)]
    [TestCase(0)]
    public void TestAlpha(float alpha)
    {
        AddStep("Change alpha", () =>
        {
            Child.Alpha = alpha;
        });
    }

    [TestCase(RED)]
    [TestCase(GREEN)]
    [TestCase(BLUE)]
    [TestCase("#FFFFFFFF")] // white
    [TestCase("#000000FF")] // black
    [TestCase("#00000000")] // not visible
    public void TestColour(string colour)
    {
        AddStep("Change alpha", () =>
        {
            Child.Colour = Color4Extensions.FromHex(colour);
        });
    }
}
