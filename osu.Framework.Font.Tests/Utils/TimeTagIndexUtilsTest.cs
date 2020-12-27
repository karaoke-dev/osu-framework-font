// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Utils;

namespace osu.Framework.Tests.Utils
{
    public class TimeTagIndexUtilsTest
    {
        [TestCase(0, TimeTagIndex.IndexState.Start, 0, 1, 0)]
        [TestCase(0, TimeTagIndex.IndexState.End, 0, 1, 0)]
        [TestCase(0, TimeTagIndex.IndexState.Start, -1, 1, 0)] // test with start navigation.
        [TestCase(0, TimeTagIndex.IndexState.Start, -2, -1, -1)] // test with end navigation.
        [TestCase(0, TimeTagIndex.IndexState.Start, 3, 1, null)] // test switch value.
        public void TestClamp(int index, TimeTagIndex.IndexState state, int minIndex, int maxIndex, int? actualIndex)
        {
            var timeTagIndex = new TimeTagIndex(index, state);

            if (actualIndex != null)
            {
                var actualTimeTagindex = new TimeTagIndex(actualIndex.Value, state);
                Assert.AreEqual(TimeTagIndexUtils.Clamp(timeTagIndex, minIndex, maxIndex), actualTimeTagindex);
            }
            else
            {
                Assert.Throws<ArgumentException>(() => TimeTagIndexUtils.Clamp(timeTagIndex, minIndex, maxIndex));
            }
        }
    }
}
