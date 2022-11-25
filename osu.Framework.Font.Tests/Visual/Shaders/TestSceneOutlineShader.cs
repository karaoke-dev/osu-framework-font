// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Font.Tests.Visual.Shaders;

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
}
