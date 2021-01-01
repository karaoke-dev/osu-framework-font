// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Utils;

namespace osu.Framework.Tests.Utils
{
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
    }
}
