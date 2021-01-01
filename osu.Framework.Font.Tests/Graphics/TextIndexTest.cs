// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;

namespace osu.Framework.Tests.Graphics
{
    [TestFixture]
    public class TextIndexTest
    {
        [TestCase(1, 1)]
        [TestCase(1.5, 1.5)]
        [TestCase(-1.5, -1.5)]
        public void TestOperatorEqual(double tone1, double tone2)
        {
            Assert.AreEqual(numberToTextIndex(tone1), numberToTextIndex(tone2));
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        [TestCase(1.5, -1.5)]
        [TestCase(1.5, 1)]
        [TestCase(-1.5, -1)]
        [TestCase(-1.5, -2)]
        public void TestOperatorNotEqual(double tone1, double tone2)
        {
            Assert.AreNotEqual(numberToTextIndex(tone1), numberToTextIndex(tone2));
        }

        [TestCase(1, 0, true)]
        [TestCase(1, 0.5, true)]
        [TestCase(1, 1, false)]
        [TestCase(1, 1.5, false)]
        public void TestOperatorGreater(double tone1, double tone2, bool match)
        {
            Assert.AreEqual(numberToTextIndex(tone1) > numberToTextIndex(tone2), match);
        }

        [TestCase(1, 0, true)]
        [TestCase(1, 0.5, true)]
        [TestCase(1, 1, true)]
        [TestCase(1, 1.5, false)]
        public void TestOperatorGreaterOrEqual(double tone1, double tone2, bool match)
        {
            Assert.AreEqual(numberToTextIndex(tone1) >= numberToTextIndex(tone2), match);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, -0.5, true)]
        [TestCase(-1, -1, false)]
        [TestCase(-1, -1.5, false)]
        public void TestOperatorLess(double tone1, double tone2, bool match)
        {
            Assert.AreEqual(numberToTextIndex(tone1) < numberToTextIndex(tone2), match);
        }

        [TestCase(-1, 0, true)]
        [TestCase(-1, -0.5, true)]
        [TestCase(-1, -1, true)]
        [TestCase(-1, -1.5, false)]
        public void TestOperatorLessOrEqual(double tone1, double tone2, bool match)
        {
            Assert.AreEqual(numberToTextIndex(tone1) <= numberToTextIndex(tone2), match);
        }

        private TextIndex numberToTextIndex(double tone)
        {
            var half = Math.Abs(tone) % 1 == 0.5;
            var scale = tone < 0 ? (int)tone - (half ? 1 : 0) : (int)tone;
            return new TextIndex(scale, half ? TextIndex.IndexState.End : TextIndex.IndexState.Start);
        }
    }
}
