// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osuTK;

namespace osu.Framework.Font.Tests.Shaders
{
    public class StepShaderTest
    {
        [Test]
        public void TestComputeCharacterDrawRectangle()
        {
            const int outline_radius = 3;
            const int shadow_offset_x = 4;
            const int shadow_offset_y = 5;

            var shader = new StepShader
            {
                StepShaders = new ICustomizedShader[]
                {
                    new OutlineShader
                    {
                        Radius = outline_radius
                    },
                    new ShadowShader
                    {
                        ShadowOffset = new Vector2(shadow_offset_x, shadow_offset_y)
                    }
                }
            };

            var rectangle = shader.ComputeCharacterDrawRectangle(new RectangleF(5, 5, 10, 10));
            Assert.AreEqual(5 - outline_radius, rectangle.X);
            Assert.AreEqual(5 - outline_radius, rectangle.Y);
            Assert.AreEqual(10 + outline_radius * 2, rectangle.Width);
            Assert.AreEqual(10 + outline_radius * 2, rectangle.Height);
        }

        [Test]
        public void TestComputeScreenSpaceDrawQuad()
        {
            const int outline_radius = 3;
            const int shadow_offset_x = 4;
            const int shadow_offset_y = 5;

            var shader = new StepShader
            {
                StepShaders = new ICustomizedShader[]
                {
                    new OutlineShader
                    {
                        Radius = outline_radius
                    },
                    new ShadowShader
                    {
                        ShadowOffset = new Vector2(shadow_offset_x, shadow_offset_y)
                    }
                }
            };

            var rectangle = shader.ComputeDrawRectangle(new RectangleF(5, 5, 10, 10));
            Assert.AreEqual(5 - outline_radius, rectangle.X);
            Assert.AreEqual(5 - outline_radius, rectangle.Y);
            Assert.AreEqual(10 + outline_radius * 2 + shadow_offset_x, rectangle.Width);
            Assert.AreEqual(10 + outline_radius * 2 + shadow_offset_y, rectangle.Height);
        }

        [Test]
        public void TestComputeCharacterDrawRectangleWithEmptyShader()
        {
            var shader = new StepShader();

            var rectangle = shader.ComputeCharacterDrawRectangle(new RectangleF(5, 5, 10, 10));
            Assert.AreEqual(5, rectangle.X);
            Assert.AreEqual(5, rectangle.Y);
            Assert.AreEqual(10, rectangle.Width);
            Assert.AreEqual(10, rectangle.Height);
        }

        [Test]
        public void TestEmptyComputeScreenSpaceDrawQuadWithEmptyShader()
        {
            var shader = new StepShader();

            var rectangle = shader.ComputeDrawRectangle(new RectangleF(5, 5, 10, 10));
            Assert.AreEqual(5, rectangle.X);
            Assert.AreEqual(5, rectangle.Y);
            Assert.AreEqual(10, rectangle.Width);
            Assert.AreEqual(10, rectangle.Height);
        }

        [Test]
        public void TestCharacterDrawRectangleWithNoneMatchedShader()
        {
            var shader = new StepShader
            {
                StepShaders = new[]
                {
                    new PixelShader()
                }
            };

            var rectangle = shader.ComputeCharacterDrawRectangle(new RectangleF(5, 5, 10, 10));
            Assert.AreEqual(5, rectangle.X);
            Assert.AreEqual(5, rectangle.Y);
            Assert.AreEqual(10, rectangle.Width);
            Assert.AreEqual(10, rectangle.Height);
        }

        [Test]
        public void TestEmptyComputeScreenSpaceDrawQuadNoneMatchedShader()
        {
            var shader = new StepShader
            {
                StepShaders = new[]
                {
                    new PixelShader()
                }
            };

            var rectangle = shader.ComputeDrawRectangle(new RectangleF(5, 5, 10, 10));
            Assert.AreEqual(5, rectangle.X);
            Assert.AreEqual(5, rectangle.Y);
            Assert.AreEqual(10, rectangle.Width);
            Assert.AreEqual(10, rectangle.Height);
        }
    }
}


