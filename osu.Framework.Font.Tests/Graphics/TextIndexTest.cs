// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Font.Tests.Helper;

namespace osu.Framework.Font.Tests.Graphics;

[TestFixture]
public class TextIndexTest
{
    [TestCase("1,start")]
    [TestCase("1,end")]
    [TestCase("-1,start")]
    public void TestOperatorEqual(string textIndex1)
    {
        Assert.AreEqual(TestCaseTextIndexHelper.ParseTextIndex(textIndex1), TestCaseTextIndexHelper.ParseTextIndex(textIndex1));
    }

    [TestCase("-1,start", "1,start")]
    [TestCase("1,start", "-1,start")]
    [TestCase("1,end", "-1,end")]
    [TestCase("1,end", "1,start")]
    [TestCase("-2,end", "-1,start")]
    [TestCase("-2,end", "-2,start")]
    public void TestOperatorNotEqual(string textIndex1, string textIndex2)
    {
        Assert.AreNotEqual(TestCaseTextIndexHelper.ParseTextIndex(textIndex1), TestCaseTextIndexHelper.ParseTextIndex(textIndex2));
    }

    [TestCase("1,start", "0,start", true)]
    [TestCase("1,start", "0,end", true)]
    [TestCase("1,start", "1,start", false)]
    [TestCase("1,start", "1,end", false)]
    public void TestOperatorGreater(string textIndex1, string textIndex2, bool match)
    {
        Assert.AreEqual(TestCaseTextIndexHelper.ParseTextIndex(textIndex1) > TestCaseTextIndexHelper.ParseTextIndex(textIndex2), match);
    }

    [TestCase("1,start", "0,start", true)]
    [TestCase("1,start", "0,end", true)]
    [TestCase("1,start", "1,start", true)]
    [TestCase("1,start", "1,end", false)]
    public void TestOperatorGreaterOrEqual(string textIndex1, string textIndex2, bool match)
    {
        Assert.AreEqual(TestCaseTextIndexHelper.ParseTextIndex(textIndex1) >= TestCaseTextIndexHelper.ParseTextIndex(textIndex2), match);
    }

    [TestCase("-1,start", "0,start", true)]
    [TestCase("-1,start", "-1,end", true)]
    [TestCase("-1,start", "-1,start", false)]
    [TestCase("-1,start", "-2,end", false)]
    public void TestOperatorLess(string textIndex1, string textIndex2, bool match)
    {
        Assert.AreEqual(TestCaseTextIndexHelper.ParseTextIndex(textIndex1) < TestCaseTextIndexHelper.ParseTextIndex(textIndex2), match);
    }

    [TestCase("-1,start", "0,start", true)]
    [TestCase("-1,start", "-1,end", true)]
    [TestCase("-1,start", "-1,start", true)]
    [TestCase("-1,start", "-2,end", false)]
    public void TestOperatorLessOrEqual(string textIndex1, string textIndex2, bool match)
    {
        Assert.AreEqual(TestCaseTextIndexHelper.ParseTextIndex(textIndex1) <= TestCaseTextIndexHelper.ParseTextIndex(textIndex2), match);
    }
}
