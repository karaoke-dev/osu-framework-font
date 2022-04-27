// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Sprites
{
    public class TestSceneKaraokeSpriteText : TestScene
    {
        private readonly ManualClock manualClock = new();
        private readonly KaraokeSpriteText karaokeSpriteText;

        public TestSceneKaraokeSpriteText()
        {
            Child = karaokeSpriteText = new KaraokeSpriteText
            {
                Text = "カラオケ！",
                Rubies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:か", "[2,3]:お" }),
                Romajies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[1,2]:ra", "[3,4]:ke" }),
                LeftTextColour = Color4.Green,
                RightTextColour = Color4.Red,
            };
        }

        [TestCase(false)]
        [TestCase(true)]
        public void TestKaraokeSpriteTextClock(bool manualTime)
        {
            if (manualTime)
            {
                AddSliderStep("Here can adjust time", 0, 3000, 1000, time =>
                {
                    manualClock.CurrentTime = time;
                });
            }

            AddStep("Apply time-tags", () =>
            {
                var startTime = getStartTime();

                karaokeSpriteText.Clock = manualTime ? new FramedClock(manualClock) : Clock;
                karaokeSpriteText.TimeTags = new Dictionary<TextIndex, double>
                {
                    { new TextIndex(0), startTime + 500 },
                    { new TextIndex(1), startTime + 600 },
                    { new TextIndex(2), startTime + 1000 },
                    { new TextIndex(3), startTime + 1500 },
                    { new TextIndex(4), startTime + 2000 },
                };
            });

            double getStartTime()
                => manualTime ? 0 : Clock.CurrentTime;
        }

        [Test]
        public void TestChangeText()
        {
            AddStep("Change texr", () =>
            {
                karaokeSpriteText.Text = "カラオケー";
            });
        }

        [Test]
        public void TestChangeRuby()
        {
            AddStep("Change ruby", () =>
            {
                var rubyTags = new[] { "[0,1]:123aaa", "[1,2]:ら", "[2,3]:お", "[3,4]:け" };
                var ruby = TestCaseTagHelper.ParseParsePositionTexts(rubyTags);
                karaokeSpriteText.Rubies = ruby;
            });
        }

        [Test]
        public void TestChangeRomaji()
        {
            AddStep("Change romaji", () =>
            {
                var romajiTags = new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" };
                var romajies = TestCaseTagHelper.ParseParsePositionTexts(romajiTags);
                karaokeSpriteText.Romajies = romajies;
            });
        }

        [Test]
        public void TestChangeFont()
        {
            AddStep("Change font", () =>
            {
                var font = FontUsage.Default.With(size: 64);
                karaokeSpriteText.Font = font;
            });
        }

        [Test]
        public void TestChangeRubyFont()
        {
            AddStep("Change ruby font", () =>
            {
                var font = FontUsage.Default.With(size: 24);
                karaokeSpriteText.RubyFont = font;
            });
        }

        [Test]
        public void TestChangeRomajiFont()
        {
            AddStep("Change romaji font", () =>
            {
                var font = FontUsage.Default.With(size: 24);
                karaokeSpriteText.RomajiFont = font;
            });
        }

        [Test]
        public void TestLeftTextColour()
        {
            AddStep("Change left text colour", () =>
            {
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

        [Test]
        public void TestRubyAlignment()
        {
            AddStep("Test ruby alignment", () =>
            {
                karaokeSpriteText.RubyAlignment = LyricTextAlignment.EqualSpace;
            });
        }

        [Test]
        public void TestRomajiAlignment()
        {
            AddStep("Test romaji alignment", () =>
            {
                karaokeSpriteText.RomajiAlignment = LyricTextAlignment.EqualSpace;
            });
        }

        [Test]
        public void TestSpacing()
        {
            AddStep("Change spacing", () =>
            {
                karaokeSpriteText.Spacing = new Vector2(10);
            });
        }

        [Test]
        public void TestRubySpacing()
        {
            AddStep("Change ruby spacing", () =>
            {
                karaokeSpriteText.RubySpacing = new Vector2(10);
            });
        }

        [Test]
        public void TestRomajiSpacing()
        {
            AddStep("Change romaji spacing", () =>
            {
                karaokeSpriteText.RomajiSpacing = new Vector2(10);
            });
        }

        [Test]
        public void TestRubyMargin()
        {
            AddStep("Change ruby margin", () =>
            {
                karaokeSpriteText.RubyMargin = 10;
            });
        }

        [Test]
        public void TestRomajiMargin()
        {
            AddStep("Change romaji margin", () =>
            {
                karaokeSpriteText.RomajiMargin = 10;
            });
        }
    }
}
