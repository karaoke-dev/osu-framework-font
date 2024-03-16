// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osuTK;

namespace osu.Framework.Font.Tests.Shaders;

public class ShadowShaderTest
{
    private const float offset_x = 4;
    private const float offset_y = 5;

    [TestCase(offset_x, 0, 5, 5, 10 + offset_x, 10)] // Offset right
    [TestCase(-offset_x, 0, 5 - offset_x, 5, 10 + offset_x, 10)] // Offset left
    [TestCase(0, offset_y, 5, 5, 10, 10 + offset_y)] // Offset down
    [TestCase(0, -offset_y, 5, 5 - offset_y, 10, 10 + offset_y)] // Offset up
    public void TestComputeScreenSpaceDrawQuad(float offsetX, float offsetY, float expectedX, float expectedY, float expectedWidth, float expectedHeight)
    {
        var shader = new ShadowShader
        {
            ShadowOffset = new Vector2(offsetX, offsetY),
        };

        var rectangle = shader.ComputeDrawRectangle(new RectangleF(5, 5, 10, 10));
        Assert.AreEqual(expectedX, rectangle.X);
        Assert.AreEqual(expectedY, rectangle.Y);
        Assert.AreEqual(expectedWidth, rectangle.Width);
        Assert.AreEqual(expectedHeight, rectangle.Height);
    }
}
