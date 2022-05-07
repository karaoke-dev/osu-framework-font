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
            new[] { "[0,start]:500", "[0,end]:600" })] // there's no need to add the interpolation time-tags.
        [TestCase(new[] { "[-1,start]:500", "[4,start]:600" },
            new string[] { })] // will filter those out-of-range time-tags.
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
        public void TestGetInterpolatedTimeTags(string[] timeTags, string[] expectedTimeTags)
        {
            var karaokeSpriteText = new KaraokeSpriteText
            {
                Text = "カラオケ",
                TimeTags = TestCaseTagHelper.ParseTimeTags(timeTags)
            };

            var expectedInterpolatedTimeTags = TestCaseTagHelper.ParseTimeTags(expectedTimeTags);
            var actualInterpolatedTimeTags = karaokeSpriteText.GetInterpolatedTimeTags();

            Assert.AreEqual(expectedInterpolatedTimeTags, actualInterpolatedTimeTags);
        }
    }
}
