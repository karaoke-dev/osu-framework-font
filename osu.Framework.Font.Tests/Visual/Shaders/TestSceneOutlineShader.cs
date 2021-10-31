// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Font.Tests.Visual.Shaders
{
    public class TestSceneOutlineShader : TestSceneInternalShader
    {
        [TestCase(0, "#FF0000")]
        [TestCase(10, "#00FF00")]
        [TestCase(20, "#0000FF")]
        [TestCase(100, "#0000FF")] // it might cause performance issue if set radius too large.
        public void TestProperty(int radius, string colour)
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
        public void CreateAnglePositionParams(int samples)
        {
            // it's a script on creating const params in shader.

            const float pi = 3.14159265359f;
            double angle = 0.0f;

            Console.WriteLine($"const lowp vec2 angelPosition[{samples}] = lowp vec2[](");

            for (int i = 0; i < samples; i++)
            {
                var last = i == samples - 1;
                angle += 1.0 / (samples / 2.0) * pi;
                Console.WriteLine($"    lowp vec2({Math.Sin(angle):N2}, {Math.Cos(angle):N2})" + (last ? "" : ","));
            }

            Console.WriteLine(");");
        }
    }
}
