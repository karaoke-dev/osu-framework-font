// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

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
    }
}
