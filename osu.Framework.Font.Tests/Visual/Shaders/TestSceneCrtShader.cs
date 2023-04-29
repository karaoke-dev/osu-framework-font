// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
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
                GetShaderByType<CrtShader>()
            };
        });
    }
}
