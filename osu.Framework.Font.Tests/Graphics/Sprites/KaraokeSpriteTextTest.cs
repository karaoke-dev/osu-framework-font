// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Sprites;

namespace osu.Framework.Font.Tests.Graphics.Sprites
{
    public class KaraokeSpriteTextTest
    {
        [TestCase(new[] { "[0,start]:500", "[0,end]:600" },
            new[] { "[0,start]:500", "[0,end]:600" })]
        [TestCase(new[] { "[-1,start]:500", "[4,start]:600" },
            new string[] { })] // will filter those out-of-range time-tags.
        public void TestGetInTheRangeTimeTags(string[] timeTags, string[] expectedTimeTags)
        {
            var expectedInterpolatedTimeTags = TestCaseTagHelper.ParseTimeTags(expectedTimeTags);
            var actualInterpolatedTimeTags = KaraokeSpriteText.GetInTheRangeTimeTags(TestCaseTagHelper.ParseTimeTags(timeTags), "カラオケ");

            Assert.AreEqual(expectedInterpolatedTimeTags, actualInterpolatedTimeTags);
        }

        [TestCase(new[] { "[0,start]:500", "[0,end]:600" },
            new[] { "[0,start]:500", "[0,end]:600" })]
        [TestCase(new[] { "[-1,start]:500", "[4,start]:600" },
            new[] { "[-1,start]:500", "[4,start]:600" })] // Not care about those out-of-range time-tags.
        [TestCase(new[] { "[0,start]:500", "[0,start]:600" },
            new[] { "[0,start]:500" })] // will remain the first time-tag even time is different.
        [TestCase(new[] { "[0,start]:500", "[0,start]:600", "[0,start]:700" },
            new[] { "[0,start]:500" })] // will remain the first time-tag.
        [TestCase(new[] { "[0,start]:500", "[0,start]:600", "[0,end]:700" },
            new[] { "[0,start]:500", "[0,end]:700" })] // will remove the second time-tag.
        [TestCase(new[] { "[0,end]:700", "[0,start]:600", "[0,start]:500" },
            new[] { "[0,start]:500", "[0,end]:700" })] // will remove the second time-tag even the time-tag order has been reversed.
        public void TestGetNonDuplicatedTimeTags(string[] timeTags, string[] expectedTimeTags)
        {
            var expectedInterpolatedTimeTags = TestCaseTagHelper.ParseTimeTags(expectedTimeTags);
            var actualInterpolatedTimeTags = KaraokeSpriteText.GetNonDuplicatedTimeTags(TestCaseTagHelper.ParseTimeTags(timeTags));

            Assert.AreEqual(expectedInterpolatedTimeTags, actualInterpolatedTimeTags);
        }

        [TestCase(new[] { "[0,start]:500", "[0,end]:600" },
            new[] { "[0,start]:500", "[0,end]:600" })] // there's no need to add the interpolation time-tags.
        [TestCase(new[] { "[0,start]:500", "[1,start]:501" },
            new[] { "[0,start]:500", "[1,start]:501" })] // there's no need to add the interpolation time-tags because timing is too small.
        [TestCase(new[] { "[0,start]:500", "[2,start]:600" },
            new[] { "[0,start]:500", "[1,end]:599", "[2,start]:600" })] // It's time to add the interpolation time-tags.
        [TestCase(new[] { "[0,end]:500", "[1,end]:600" },
            new[] { "[0,end]:500", "[1,start]:501", "[1,end]:600" })]
        [TestCase(new[] { "[0,end]:500", "[2,start]:600" },
            new[] { "[0,end]:500", "[1,start]:501", "[1,end]:599", "[2,start]:600" })]
        [TestCase(new[] { "[0,end]:500", "[3,start]:600" },
            new[] { "[0,end]:500", "[1,start]:501", "[2,end]:599", "[3,start]:600" })]
        [TestCase(new[] { "[2,start]:500", "[0,start]:600" },
            new[] { "[2,start]:500", "[1,end]:501", "[0,start]:600" })] // let's test some reverse state
        [TestCase(new[] { "[1,end]:500", "[0,end]:600" },
            new[] { "[1,end]:500", "[1,start]:599", "[0,end]:600" })]
        [TestCase(new[] { "[2,start]:500", "[0,end]:600" },
            new[] { "[2,start]:500", "[1,end]:501", "[1,start]:599", "[0,end]:600" })]
        [TestCase(new[] { "[3,start]:500", "[0,end]:600" },
            new[] { "[3,start]:500", "[2,end]:501", "[1,start]:599", "[0,end]:600" })]
        [TestCase(new[] { "[-1,start]:500", "[4,start]:600" },
            new[] { "[-1,start]:500", "[3,end]:599", "[4,start]:600" })] // Not care about those out-of-range time-tags.
        public void TestGetInterpolatedTimeTags(string[] timeTags, string[] expectedTimeTags)
        {
            var expectedInterpolatedTimeTags = TestCaseTagHelper.ParseTimeTags(expectedTimeTags);
            var actualInterpolatedTimeTags = KaraokeSpriteText.GetInterpolatedTimeTags(TestCaseTagHelper.ParseTimeTags(timeTags));

            Assert.AreEqual(expectedInterpolatedTimeTags, actualInterpolatedTimeTags);
        }
    }
}
