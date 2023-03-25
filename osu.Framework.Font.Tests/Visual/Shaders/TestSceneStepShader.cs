// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Shaders;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Shaders;

public partial class TestSceneStepShader : ShaderTestScene
{
    [Test]
    public void TestShaderInStepShader()
    {
        for (int i = 0; i < 10; i++)
        {
            var shaderAmount = i;
            AddStep($"Apply shader amount: {shaderAmount}", () =>
            {
                ShaderContainer.Shaders = new[]
                {
                    new StepShader
                    {
                        Name = "Step 1",
                        StepShaders = Enumerable.Repeat(default(int), shaderAmount).Select((_, index) =>
                        {
                            var shader = GetShaderByType<OutlineShader>();
                            var hue = (float)index * 30 / 360;

                            shader.Radius = 5;
                            shader.OutlineColour = Color4.FromHsl(new Vector4(hue, 0.6f, 0.5f, 1));

                            return shader;
                        }).ToArray()
                    }
                };
            });
        }
    }

    [Test]
    public void TestDrawStepShader()
    {
        var stepShader = new StepShader
        {
            Name = "Step 1",
            StepShaders = new[]
            {
                GetShaderByType<OutlineShader>().With(s =>
                {
                    s.Radius = 3;
                    s.OutlineColour = Color4.Yellow;
                }),
                GetShaderByType<OutlineShader>().With(s =>
                {
                    s.Radius = 3;
                    s.OutlineColour = Color4.Red;
                })
            }
        };

        AddStep("Apply shader", () =>
        {
            ShaderContainer.Shaders = new[]
            {
                stepShader
            };
        });

        AddStep("Not drawing", () =>
        {
            stepShader.Draw = false;
        });

        AddStep("Drawing", () =>
        {
            stepShader.Draw = true;
        });
    }

    [Test]
    public void TestDrawStepShaderReferenceShader()
    {
        var outlineStepShader = new StepShader
        {
            Name = "Step create outline",
            StepShaders = new[]
            {
                GetShaderByType<OutlineShader>().With(s =>
                {
                    s.Radius = 3;
                    s.OutlineColour = Color4.Yellow;
                })
            }
        };

        var shadowStepShader = new StepShader
        {
            Name = "Step create shadow",
            StepShaders = new[]
            {
                GetShaderByType<ShadowShader>().With(s =>
                {
                    s.ShadowColour = Color4.Red;
                    s.ShadowOffset = new Vector2(10);
                })
            }
        };

        AddStep("Apply shader", () =>
        {
            ShaderContainer.Shaders = new[]
            {
                outlineStepShader,
                shadowStepShader
            };
        });

        AddStep("Shadow shader should after outline", () =>
        {
            shadowStepShader.FromShader = outlineStepShader;

            // should re-assign shaders.
            ShaderContainer.Shaders = new[]
            {
                outlineStepShader,
                shadowStepShader
            };
        });

        AddStep("Shadow shader not after outline", () =>
        {
            shadowStepShader.FromShader = null;

            // should re-assign shaders.
            ShaderContainer.Shaders = new[]
            {
                outlineStepShader,
                shadowStepShader
            };
        });
    }
}
