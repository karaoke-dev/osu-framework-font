// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Sprites;

namespace osu.Framework.Font.Tests.Graphics.Sprites
{
    public class KaraokeSpriteTextTest
    {
        [TestCase(new[] { "[0,start]:500", "[1,start]:600", "[2,start]:1000", "[3,start]:1500" }, new[] { "[0,start]:500", "[1,start]:600", "[2,start]:1000", "[3,start]:1500" })] // there's no need to add the interpolation time-tags.
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
