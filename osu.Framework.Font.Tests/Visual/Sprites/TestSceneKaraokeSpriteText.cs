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
                Rubies = new[]
                {
                    new PositionText
                    {
                        StartIndex = 0,
                        EndIndex = 1,
                        Text = "か"
                    },
                    new PositionText
                    {
                        StartIndex = 2,
                        EndIndex = 3,
                        Text = "お"
                    }
                },
                Romajies = new[]
                {
                    new PositionText
                    {
                        StartIndex = 1,
                        EndIndex = 2,
                        Text = "ra"
                    },
                    new PositionText
                    {
                        StartIndex = 3,
                        EndIndex = 4,
                        Text = "ke"
                    }
                },
                LeftTextColour = Color4.Green,
                RightTextColour = Color4.Red,
            };
        }

        [TestCase(false)]
        [TestCase(true)]
        public void TestKaraokeSpriteTextTimeTag(bool manualTime)
        {
            if (manualTime)
            {
                AddSliderStep("Here can adjust time", 0, 3000, 1000, time =>
                {
                    manualClock.CurrentTime = time;
                });
            }

            AddStep("Default time tag", () =>
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
            AddStep("Time tag with end state", () =>
            {
                var startTime = getStartTime();

                karaokeSpriteText.Clock = manualTime ? new FramedClock(manualClock) : Clock;
                karaokeSpriteText.TimeTags = new Dictionary<TextIndex, double>
                {
                    // カ
                    { new TextIndex(0), startTime + 0 },
                    { new TextIndex(0, TextIndex.IndexState.End), startTime + 100 },
                    // ラ
                    { new TextIndex(1), startTime + 1000 },
                    { new TextIndex(1, TextIndex.IndexState.End), startTime + 1100 },
                    // オ
                    { new TextIndex(2), startTime + 2000 },
                    { new TextIndex(2, TextIndex.IndexState.End), startTime + 2100 },
                    // ケ
                    { new TextIndex(3), startTime + 3000 },
                    { new TextIndex(3, TextIndex.IndexState.End), startTime + 3100 },
                    // !
                    { new TextIndex(4), startTime + 4000 },
                    { new TextIndex(4, TextIndex.IndexState.End), startTime + 4100 },
                };
            });
            AddStep("Time tag with wrong order", () =>
            {
                var startTime = getStartTime();

                karaokeSpriteText.Clock = manualTime ? new FramedClock(manualClock) : Clock;
                karaokeSpriteText.TimeTags = new Dictionary<TextIndex, double>
                {
                    { new TextIndex(4), startTime + 2000 },
                    { new TextIndex(3), startTime + 1500 },
                    { new TextIndex(2), startTime + 1000 },
                    { new TextIndex(1), startTime + 600 },
                    { new TextIndex(0), startTime + 500 },
                };
            });
            AddStep("Time tag with out of range", () =>
            {
                var startTime = getStartTime();

                karaokeSpriteText.Clock = manualTime ? new FramedClock(manualClock) : Clock;
                karaokeSpriteText.TimeTags = new Dictionary<TextIndex, double>
                {
                    { new TextIndex(-1), startTime + 0 },
                    { new TextIndex(0), startTime + 500 },
                    { new TextIndex(1), startTime + 600 },
                    { new TextIndex(2), startTime + 1000 },
                    { new TextIndex(3), startTime + 1500 },
                    { new TextIndex(4), startTime + 2000 },
                    { new TextIndex(8), startTime + 2500 },
                };
            });

            AddStep("Only one time-tag", () =>
            {
                var startTime = getStartTime();

                karaokeSpriteText.Clock = manualTime ? new FramedClock(manualClock) : Clock;
                karaokeSpriteText.TimeTags = new Dictionary<TextIndex, double>
                {
                    { new TextIndex(0), startTime + 500 },
                };
            });

            AddStep("None time-tag", () =>
            {
                karaokeSpriteText.Clock = manualTime ? new FramedClock(manualClock) : Clock;
                karaokeSpriteText.TimeTags = null;
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
    }
}
