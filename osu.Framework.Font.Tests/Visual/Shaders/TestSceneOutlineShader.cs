// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Font.Tests.Visual.Shaders
{
    public class TestSceneOutlineShader : InternalShaderTestScene
    {
        [TestCase(RED)] // will make the inner texture red
        [TestCase(GREEN)] // will make the inner texture green
        [TestCase(BLUE)] // will make the inner texture blur
        [TestCase("#FFFFFFFF")] // will make the inner texture white
        [TestCase("#000000FF")] // will make the inner texture black
        [TestCase("#00000000")] // will not affect inner texture
        [TestCase("#FFFFFF00")] // will not affect inner texture
        [TestCase("#FFFFFFAA")] // will make inner texture lighter
        public void TestColour(string colour)
        {
            AddStep("Apply shader", () =>
            {
                ShaderContainer.Shaders = new[]
                {
                    GetShaderByType<OutlineShader>().With(s =>
                    {
                        s.Colour = Color4Extensions.FromHex(colour);
                    })
                };
            });
        }

        [TestCase(0, RED)]
        [TestCase(10, GREEN)]
        [TestCase(20, BLUE)]
        [TestCase(100, BLUE)] // it might cause performance issue if set radius too large.
        public void TestOutline(int radius, string colour)
        {
            AddStep("Apply shader", () =>
            {
                ShaderContainer.Shaders = new[]
                {
                    GetShaderByType<OutlineShader>().With(s =>
                    {
                        s.Radius = radius;
                        s.OutlineColour = Color4Extensions.FromHex(colour);
                    })
                };
            });
        }

        [TestCase(128)]
        public void CreateGetOutlineMethod(int samples)
        {
            // it's a script on creating const params in shader.

            const float pi = 3.14159265359f;
            double angle = 0.0f;

            Console.WriteLine($"lowp vec4 outline(sampler2D tex, int radius, mediump vec2 texCoord, mediump vec2 texSize, mediump vec4 colour)");
            Console.WriteLine("{");
            Console.WriteLine("    mediump vec2 offset = mediump vec2(float(radius)) / texSize;");
            Console.WriteLine("    float alpha = 0.0;");
            Console.WriteLine();

            for (int i = 0; i < samples; i++)
            {
                angle += 1.0 / (samples / 2.0) * pi;
                Console.WriteLine($"    alpha = max(alpha, texture2D(tex, texCoord - lowp vec2({Math.Sin(angle):N2}, {Math.Cos(angle):N2}) * offset).a);");
            }

            Console.WriteLine("    return mix(vec4(0.0), colour, alpha);");
            Console.WriteLine("}");
        }
    }
}
