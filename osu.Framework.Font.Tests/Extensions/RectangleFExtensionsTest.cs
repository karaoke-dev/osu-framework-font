// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Extensions;
using osu.Framework.Graphics.Primitives;

namespace osu.Framework.Font.Tests.Extensions
{
    public class RectangleFExtensionsTest
    {
        [TestCase(2, Anchor.Centre, 0, 5, 40, 20)]
        [TestCase(2, Anchor.TopLeft, 10, 10, 40, 20)]
        [TestCase(2, Anchor.TopRight, -10, 10, 40, 20)]
        [TestCase(2, Anchor.BottomLeft, 10, 0, 40, 20)]
        [TestCase(2, Anchor.BottomRight, -10, 0, 40, 20)]
        public void TestScale(float scale, Anchor origin, float x, float y, float width, float height)
        {
            var rectangle = new RectangleF(10, 10, 20, 10);
            var target = rectangle.Scale(scale, origin);

            Assert.AreEqual(target.X, x);
            Assert.AreEqual(target.Y, y);
            Assert.AreEqual(target.Width, width);
            Assert.AreEqual(target.Height, height);
        }
    }
}
