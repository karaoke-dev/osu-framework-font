// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Utils;

namespace osu.Framework.Font.Tests.Utils;

public class TextIndexUtilsTest
{
    [TestCase(0, TextIndex.IndexState.Start, 0, 1, 0)]
    [TestCase(0, TextIndex.IndexState.End, 0, 1, 0)]
    [TestCase(0, TextIndex.IndexState.Start, -1, 1, 0)] // test with start navigation.
    [TestCase(0, TextIndex.IndexState.Start, -2, -1, -1)] // test with end navigation.
    [TestCase(0, TextIndex.IndexState.Start, 3, 1, null)] // test switch value.
    public void TestClamp(int index, TextIndex.IndexState state, int minIndex, int maxIndex, int? actualIndex)
    {
        var textIndex = new TextIndex(index, state);

        if (actualIndex != null)
        {
            var actualTextIndex = new TextIndex(actualIndex.Value, state);
            Assert.AreEqual(TextIndexUtils.Clamp(textIndex, minIndex, maxIndex), actualTextIndex);
        }
        else
        {
            Assert.Throws<ArgumentException>(() => TextIndexUtils.Clamp(textIndex, minIndex, maxIndex));
        }
    }

    [TestCase(0, TextIndex.IndexState.Start, 0)]
    [TestCase(0, TextIndex.IndexState.End, 1)]
    [TestCase(-1, TextIndex.IndexState.Start, -1)] // In utils not checking is index out of range
    [TestCase(-1, TextIndex.IndexState.End, 0)]
    public void TestToStringIndex(int index, TextIndex.IndexState state, int expected)
    {
        var textIndex = new TextIndex(index, state);

        int actual = TextIndexUtils.ToStringIndex(textIndex);
        Assert.AreEqual(expected, actual);
    }

    [TestCase(TextIndex.IndexState.Start, TextIndex.IndexState.End)]
    [TestCase(TextIndex.IndexState.End, TextIndex.IndexState.Start)]
    public void TestReverseState(TextIndex.IndexState state, TextIndex.IndexState expected)
    {
        var actual = TextIndexUtils.ReverseState(state);
        Assert.AreEqual(expected, actual);
    }

    [TestCase(1, TextIndex.IndexState.End, 1, TextIndex.IndexState.Start)]
    [TestCase(1, TextIndex.IndexState.Start, 0, TextIndex.IndexState.End)]
    [TestCase(0, TextIndex.IndexState.Start, -1, TextIndex.IndexState.End)] // didn't care about negative value.
    [TestCase(-1, TextIndex.IndexState.End, -1, TextIndex.IndexState.Start)] // didn't care about negative value.
    public void TestGetPreviousIndex(int index, TextIndex.IndexState state, int expectedIndex, TextIndex.IndexState expectedState)
    {
        var textIndex = new TextIndex(index, state);

        var expected = new TextIndex(expectedIndex, expectedState);
        var actual = TextIndexUtils.GetPreviousIndex(textIndex);
        Assert.AreEqual(expected, actual);
    }

    [TestCase(0, TextIndex.IndexState.Start, 0, TextIndex.IndexState.End)]
    [TestCase(0, TextIndex.IndexState.End, 1, TextIndex.IndexState.Start)]
    [TestCase(-1, TextIndex.IndexState.Start, -1, TextIndex.IndexState.End)] // didn't care about negative value.
    [TestCase(-1, TextIndex.IndexState.End, 0, TextIndex.IndexState.Start)] // didn't care about negative value.
    public void TestGetNextIndex(int index, TextIndex.IndexState state, int expectedIndex, TextIndex.IndexState expectedState)
    {
        var textIndex = new TextIndex(index, state);

        var expected = new TextIndex(expectedIndex, expectedState);
        var actual = TextIndexUtils.GetNextIndex(textIndex);
        Assert.AreEqual(expected, actual);
    }
}
