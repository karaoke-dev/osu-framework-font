// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace osu.Framework.Font.Tests.Visual.Shaders;

public partial class TestSceneRepeatMovingBackgroundShader : InternalShaderTestScene
{
    [Resolved]
    private TextureStore textures { get; set; } = null!;

    [TestCase("sample-texture", 10, 10)]
    [TestCase("sample-texture", 30, 30)]
    [TestCase("sample-texture", 5, 20)]
    public void TestTextureDisplaySize(string textureName, float width, float height)
    {
        AddStep("Apply shader", () =>
        {
            ShaderContainer.Shaders = new[]
            {
                GetShaderByType<RepeatMovingBackgroundShader>().With(s =>
                {
                    s.Texture = textures.Get(textureName);
                    s.TextureDisplaySize = new Vector2(width, height);
                })
            };
        });
    }

    [TestCase("sample-texture", 0, 0)]
    [TestCase("sample-texture", 10, 10)]
    [TestCase("sample-texture", 5, 20)]
    [TestCase("sample-texture", 30, 30)]
    public void TestTextureBorder(string textureName, float width, float height)
    {
        AddStep("Apply shader", () =>
        {
            ShaderContainer.Shaders = new[]
            {
                GetShaderByType<RepeatMovingBackgroundShader>().With(s =>
                {
                    s.Texture = textures.Get(textureName);
                    s.TextureDisplaySize = new Vector2(30);
                    s.TextureDisplayBorder = new Vector2(width, height);
                })
            };
        });
    }

    [TestCase(0, 0)]
    [TestCase(1, 1)]
    [TestCase(0.1f, 0.1f)]
    [TestCase(5, 2)]
    public void TestTextureMovingSpeed(float xSpeed, float ySpeed)
    {
        AddStep("Apply shader", () =>
        {
            ShaderContainer.Shaders = new[]
            {
                GetShaderByType<RepeatMovingBackgroundShader>().With(s =>
                {
                    s.Texture = textures.Get("sample-texture");
                    s.TextureDisplaySize = new Vector2(30);
                    s.Speed = new Vector2(xSpeed, ySpeed);
                })
            };
        });
    }

    [TestCase(0)]
    [TestCase(0.5f)]
    [TestCase(1f)]
    [TestCase(-1)] // invalid
    [TestCase(3)] // invalid
    public void TestTextureMix(float mix)
    {
        AddStep("Apply shader", () =>
        {
            ShaderContainer.Shaders = new[]
            {
                GetShaderByType<RepeatMovingBackgroundShader>().With(s =>
                {
                    s.Texture = textures.Get("sample-texture");
                    s.TextureDisplaySize = new Vector2(30);
                    s.Speed = new Vector2(1);
                    s.Mix = mix;
                })
            };
        });
    }
}
