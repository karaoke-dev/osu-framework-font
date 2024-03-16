// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Font.Tests.Visual.Shaders;

public partial class TestSceneCrtShader : InternalShaderTestScene
{
    [Test]
    public void TestShader()
    {
        AddStep("Apply shader", () =>
        {
            ShaderContainer.Shaders = new[]
            {
                GetShaderByType<CrtShader>(),
            };
        });
    }

    [TestCase(RED)] // will make the background red
    [TestCase(GREEN)] // will make the background green
    [TestCase(BLUE)] // will make the background blur
    [TestCase("#FFFFFFFF")] // will make the background white
    [TestCase("#000000FF")] // will make the background black
    [TestCase("#00000000")] // have no colour in the background.
    public void TestProperty(string colour)
    {
        AddStep("Apply shader", () =>
        {
            ShaderContainer.Shaders = new[]
            {
                GetShaderByType<CrtShader>().With(s => s.BackgroundColour = Color4Extensions.FromHex(colour)),
            };
        });
    }
}
