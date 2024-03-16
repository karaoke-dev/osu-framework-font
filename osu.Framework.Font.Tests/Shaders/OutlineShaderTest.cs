// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Font.Tests.Shaders;

public class OutlineShaderTest
{
    [TestCase(10, -5, -5, 30, 30)]
    [TestCase(0, 5, 5, 10, 10)]
    [TestCase(-10, 5, 5, 10, 10)]
    public void TestComputeCharacterDrawRectangle(int radius, float expectedX, float expectedY, float expectedWidth, float expectedHeight)
    {
        var shader = new OutlineShader
        {
            Radius = radius,
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
            Radius = radius,
        };

        var rectangle = shader.ComputeDrawRectangle(new RectangleF(5, 5, 10, 10));
        Assert.AreEqual(expectedX, rectangle.X);
        Assert.AreEqual(expectedY, rectangle.Y);
        Assert.AreEqual(expectedWidth, rectangle.Width);
        Assert.AreEqual(expectedHeight, rectangle.Height);
    }

    [TestCase(32)]
    public void CreateGetOutlineMethod(int samples)
    {
        // it's a script on creating const params in shader.

        const float pi = 3.14159265359f;
        double angle = 0.0f;

        Console.WriteLine("lowp float outlineAlpha(in float radius, in mediump vec2 texCoord, in mediump vec2 texSize)");
        Console.WriteLine("{");
        Console.WriteLine("	mediump vec2 offset = vec2(radius) / texSize;");
        Console.WriteLine("	lowp float alpha = 0.0;");
        Console.WriteLine();

        for (int i = 0; i < samples; i++)
        {
            angle += 1.0 / (samples / 2.0) * pi;
            Console.WriteLine($"	alpha = max(alpha, tex(texCoord - vec2({Math.Sin(angle):N2}, {Math.Cos(angle):N2}) * offset).a);");
        }

        Console.WriteLine("	return alpha;");
        Console.WriteLine("}");
    }
}
