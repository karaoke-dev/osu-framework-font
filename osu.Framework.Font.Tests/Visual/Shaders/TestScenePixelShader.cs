// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Shaders;
using osuTK;

namespace osu.Framework.Font.Tests.Visual.Shaders;

public partial class TestScenePixelShader : InternalShaderTestScene
{
    [TestCase(5, 5)]
    [TestCase(5, 20)]
    [TestCase(20, 20)]
    public void TestProperty(float x, float y)
    {
        AddStep("Apply shader", () =>
        {
            ShaderContainer.Shaders = new[]
            {
                GetShaderByType<PixelShader>().With(s =>
                {
                    s.Size = new Vector2(x, y);
                }),
            };
        });
    }
}
