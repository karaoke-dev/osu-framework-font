// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Sprites;

namespace osu.Framework.Font.Tests.Graphics.Sprites
{
    public class LyricSpriteTextTest
    {
        [TestCase("カラオケ", "[0,1]:か", "[0,1]:か")]
        [TestCase("カラオケ", "[3,4]:か", "[3,4]:か")]
        [TestCase("カラオケ", "[-1,1]:か", "[0,1]:か")] // fix out of range issue.
        [TestCase("カラオケ", "[0,5]:か", "[0,4]:か")]
        [TestCase("カラオケ", "[1,0]:か", "[0,1]:か")] // fix the case that end index is small than start index.
        [TestCase("カラオケ", "[0,0]:か", "[0,0]:か")] // will not fix if start and end index is same.
        [TestCase("カラオケ", "[0,1]:", "[0,1]:")] // will not fix the case with no text.
        [TestCase("", "[0,0]:か", "[0,0]:か")] // should be validate
        [TestCase("", "[0,0]:か", "[0,0]:か")]
        [TestCase("", "[-1,1]:か", "[0,0]:か")] // should give it a fix.
        [TestCase("", "[-1,1]:か", "[0,0]:か")]
        public void TestGetFixedPositionText(string lyric, string positionText, string fixedPositionText)
        {
            var expected = TestCaseTagHelper.ParsePositionText(fixedPositionText);
            var actual = LyricSpriteText.GetFixedPositionText(TestCaseTagHelper.ParsePositionText(positionText), lyric);
            Assert.AreEqual(expected, actual);
        }
    }
}
