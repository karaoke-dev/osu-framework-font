// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Font.Tests.Visual.Shaders
{
    public class CustomizedShaderTestScene : ShaderTestScene
    {
        [TestCase(0, "#FF0000")]
        [TestCase(10, "#00FF00")]
        [TestCase(20, "#0000FF")]
        public void TestOutlineShader(int radius, string colour)
        {
            AddStep("Apply shader", () =>
            {
                ShaderContainer.Shaders = new[]
                {
                    new OutlineShader(GetShader("Outline"))
                    {
                        Radius = radius,
                        OutlineColour = Color4Extensions.FromHex(colour),
                    }
                };
            });
        }

        [TestCase("#FFFF00", "(10,10)")]
        [TestCase("#FF0000", "(-20,-20)")]
        public void TestShadowShader(string colour, string offset)
        {
            AddStep("Apply shader", () =>
            {
                ShaderContainer.Shaders = new[]
                {
                    new ShadowShader(GetShader("Shadow"))
                    {
                        ShadowColour = Color4Extensions.FromHex(colour),
                        ShadowOffset = TestCaseVectorHelper.ParseVector2(offset)
                    }
                };
            });
        }
    }
}
