// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Sprites;

namespace osu.Framework.Font.Tests.Graphics.Sprites;

public class LyricSpriteTextTest
{
    [TestCase("カラオケ", new[] { "[0]:か" }, new[] { "[0]:か" })]
    [TestCase("カラオケ", new[] { "[0]:" }, new[] { "[0]: " })] // for able to get the empty top/bottom position text's char index, will make the empty text with spacing instead.
    [TestCase("カラオケ", new[] { "[0]:か", "[0]:か" }, new[] { "[0]:か" })] // will filter the duplicated
    [TestCase("カラオケ", new[] { "[0]:か", "[0]:ら" }, new[] { "[0]:か", "[0]:ら" })] // will not filter even if index are same.
    [TestCase("カラオケ", new[] { "[0,1]:から", "[1,0]:から" }, new[] { "[0,1]:から" })] // will give it a fix and filter the duplicated.
    public void TestGetFixedPositionTexts(string lyric, string[] positionTexts, string[] fixedPositionTexts)
    {
        var expected = TestCaseTagHelper.ParsePositionTexts(fixedPositionTexts);
        var actual = LyricSpriteText.GetFixedPositionTexts(TestCaseTagHelper.ParsePositionTexts(positionTexts), lyric);
        Assert.AreEqual(expected, actual);
    }

    [TestCase("カラオケ", "[0]:か", "[0]:か")]
    [TestCase("カラオケ", "[0,1]:から", "[0,1]:から")]
    [TestCase("カラオケ", "[2,3]:おけ", "[2,3]:おけ")]
    [TestCase("カラオケ", "[1,0]:から", "[0,1]:から")] // fix the case that end index is small than start index.
    [TestCase("カラオケ", "[0,1]:", "[0,1]: ")] // for able to get the empty top/bottom position text's char index, will make the empty text with spacing instead.
    [TestCase("カラオケ", "[-1,1]:--", null)] // not render the position text if not in the text range.
    [TestCase("カ", "[-1,1]:か", null)]
    [TestCase("カラオケ", "[0,4]:からおけ", null)]
    [TestCase("", "[0]:か", null)] // should remove all the time-tags if string is empty.
    [TestCase("", "[-1,-1]:か", null)]
    public void TestGetFixedPositionText(string lyric, string positionText, string? fixedPositionText)
    {
        var expected = fixedPositionText != null ? TestCaseTagHelper.ParsePositionText(fixedPositionText) : default(PositionText?);
        var actual = LyricSpriteText.GetFixedPositionText(TestCaseTagHelper.ParsePositionText(positionText), lyric);
        Assert.AreEqual(expected, actual);
    }
}
