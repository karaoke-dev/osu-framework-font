// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osu.Framework.Timing;
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

        [TestCase(new[] { "[0,start]:500", "[1,start]:600", "[2,start]:1000", "[3,start]:1500", "[4,start]:2000" }, true)] // Normal time-tag.
        [TestCase(new[] { "[0,start]:0", "[0,end]:100", "[1,start]:1000", "[1,end]:1100", "[2,start]:2000", "[2,end]:2100", "[3,start]:3000", "[3,end]:3100", "[4,start]:4000", "[4,end]:4100" }, true)]
        [TestCase(new[] { "[-1,start]:0", "[0,start]:500", "[1,end]:600", "[2,start]:1000", "[3,end]:1500", "[4,end]:2000", "[8,end]:2500" }, true)] // Out-of-range time-tag, but it's acceptable now.
        [TestCase(new[] { "[0,start]:500" }, true)] // Only one time-tag.
        public void TestKaraokeSpriteTextTimeTags(string[] timeTags, bool boo)
        {
            AddStep("Apply time-tags", () =>
            {
                var startTime = Clock.CurrentTime;

                karaokeSpriteText.TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
                                                              .ToDictionary(k => k.Key, v => v.Value + startTime);
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
                var ruby = TestCaseTagHelper.ParseParsePositionTexts(rubyTags);
                karaokeSpriteText.Rubies = ruby;
            });
        }

        [TestCase(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }, true)]
        public void TestRomaji(string[] romajiTags, bool boo)
        {
            AddStep("Change romaji", () =>
            {
                var romajies = TestCaseTagHelper.ParseParsePositionTexts(romajiTags);
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
    }
}
