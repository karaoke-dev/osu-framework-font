// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;

namespace osu.Framework.Font.Tests.Visual.Shaders
{
    public class TestSceneNormalShader : ShaderTestScene
    {
        [TestCase("CRT")]
        [TestCase("not_found_shader")] // notice that missing shader will let whole sprite text being white.
        public void ApplySingleShader(string shaderName)
        {
            AddStep("Apply shader", () =>
            {
                ShaderContainer.Shaders = new[]
                {
                    GetShader(shaderName)
                };
            });
        }
    }
}
