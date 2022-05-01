// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Font.Tests.Shaders
{
    public class OutlineShaderTest
    {
        [TestCase(10, -5, -5, 30, 30)]
        [TestCase(0, 5, 5, 10, 10)]
        [TestCase(-10, 5, 5, 10, 10)]
        public void TestComputeCharacterDrawRectangle(int radius, float expectedX, float expectedY, float expectedWidth, float expectedHeight)
        {
            var shader = new OutlineShader
            {
                Radius = radius
            };

            var rectangle = shader.ComputeCharacterDrawRectangle(new RectangleF(5, 5, 10, 10));
            Assert.AreEqual(expectedX, rectangle.X);
            Assert.AreEqual(expectedY, rectangle.Y);
            Assert.AreEqual(expectedWidth, rectangle.Width);
            Assert.AreEqual(expectedHeight, rectangle.Height);
        }

        [TestCase(10, -5, -5, 30, 30)]
        [TestCase(0, 5, 5, 10, 10)]
        [TestCase(-10, 5, 5, 10, 10)]
        public void TestComputeScreenSpaceDrawQuad(int radius, float expectedX, float expectedY, float expectedWidth, float expectedHeight)
        {
            var shader = new OutlineShader
            {
                Radius = radius
            };

            var quad = shader.ComputeScreenSpaceDrawQuad(new Quad(5, 5, 10, 10));
            var rectangle = quad.AABBFloat;
            Assert.AreEqual(expectedX, rectangle.X);
            Assert.AreEqual(expectedY, rectangle.Y);
            Assert.AreEqual(expectedWidth, rectangle.Width);
            Assert.AreEqual(expectedHeight, rectangle.Height);
        }
    }
}
