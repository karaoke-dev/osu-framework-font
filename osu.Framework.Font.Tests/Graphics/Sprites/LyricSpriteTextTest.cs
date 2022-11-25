// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Sprites;

namespace osu.Framework.Font.Tests.Graphics.Sprites;

public class LyricSpriteTextTest
{
    [TestCase("カラオケ", new[] { "[0,1]:か" }, new[] { "[0,1]:か" })]
    [TestCase("カラオケ", new[] { "[0,1]:" }, new[] { "[0,1]: " })] // for able to get the empty ruby/romaji position text's position, will make the empty text with spacing instead.
    [TestCase("カラオケ", new[] { "[0,1]:か", "[0,1]:か" }, new[] { "[0,1]:か" })] // will filter the duplicated
    [TestCase("カラオケ", new[] { "[0,1]:か", "[0,1]:ら" }, new[] { "[0,1]:か", "[0,1]:ら" })] // will not filter even if index are same.
    [TestCase("カラオケ", new[] { "[0,1]:か", "[1,0]:か" }, new[] { "[0,1]:か" })] // will give it a fix and filter the duplicated.
    public void TestGetFixedPositionTexts(string lyric, string[] positionTexts, string[] fixedPositionTexts)
    {
        var expected = TestCaseTagHelper.ParsePositionTexts(fixedPositionTexts);
        var actual = LyricSpriteText.GetFixedPositionTexts(TestCaseTagHelper.ParsePositionTexts(positionTexts), lyric);
        Assert.AreEqual(expected, actual);
    }

    [TestCase("カラオケ", "[0,1]:か", "[0,1]:か")]
    [TestCase("カラオケ", "[3,4]:か", "[3,4]:か")]
    [TestCase("カラオケ", "[-1,1]:か", "[0,1]:か")] // fix out of range issue.
    [TestCase("カラオケ", "[0,5]:か", "[0,4]:か")]
    [TestCase("カラオケ", "[1,0]:か", "[0,1]:か")] // fix the case that end index is small than start index.
    [TestCase("カラオケ", "[0,0]:か", "[0,0]:か")] // will not fix if start and end index is same.
    [TestCase("カラオケ", "[0,1]:", "[0,1]: ")] // for able to get the empty ruby/romaji position text's position, will make the empty text with spacing instead.
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
