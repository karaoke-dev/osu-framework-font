// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
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
            var index1 = new TextIndex
            {
                Index = 3,
                State = TextIndex.IndexState.Start,
            };

            var index2 = new TextIndex
            {
                Index = 1,
                State = TextIndex.IndexState.Start
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
            var index1 = new TextIndex
            {
                Index = 0,
                State = TextIndex.IndexState.Start,
            };

            var index2 = new TextIndex
            {
                Index = -1,
                State = TextIndex.IndexState.Start
            };

            Assert.AreEqual(index1 > index2, true);
            Assert.AreEqual(index1 < index2, false);
        }
    }
}
