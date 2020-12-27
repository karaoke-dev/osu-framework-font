// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Sprites;

namespace osu.Framework.Tests.Visual.Sprites
{
    [TestFixture]
    public class TimeTagIndexCalculationTest
    {
        [Test]
        public void TestToneCalculation()
        {
            var index1 = new TimeTagIndex
            {
                Index = 3,
                State = TimeTagIndex.IndexState.Start,
            };

            var index2 = new TimeTagIndex
            {
                Index = 1,
                State = TimeTagIndex.IndexState.Start
            };

            Assert.AreEqual(index1 == index2, false);
            Assert.AreEqual(index1 != index2, true);

            Assert.AreEqual(index1 > index2, true);
            Assert.AreEqual(index1 >= index2, true);

            Assert.AreEqual(index1 < index2, false);
            Assert.AreEqual(index1 <= index2, false);
        }

        [Test]
        public void TestToneComparison()
        {
            var index1 = new TimeTagIndex
            {
                Index = 0,
                State = TimeTagIndex.IndexState.Start,
            };

            var index2 = new TimeTagIndex
            {
                Index = -1,
                State = TimeTagIndex.IndexState.Start
            };

            Assert.AreEqual(index1 > index2, true);
            Assert.AreEqual(index1 < index2, false);
        }
    }
}
