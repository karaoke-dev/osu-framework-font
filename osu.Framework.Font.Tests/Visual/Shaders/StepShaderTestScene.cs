// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics.Shaders;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Shaders
{
    public class StepShaderTestScene : ShaderTestScene
    {
        [Test]
        public void ApplySingleStepShader()
        {
            var stepShader = new StepShader
            {
                Name = "Step 1",
                StepShaders = new[]
                {
                    new OutlineShader(GetShader("Outline"))
                    {
                        Radius = 10,
                        OutlineColour = Color4.Yellow,
                    },
                    GetShader("CRT"),
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
        public void ApplyMultiStepShader()
        {
            var outlineStepShader = new StepShader
            {
                Name = "Step create outline",
                StepShaders = new[]
                {
                    new OutlineShader(GetShader("Outline"))
                    {
                        Radius = 10,
                        OutlineColour = Color4.Yellow,
                    },
                }
            };

            var shadowStepShader = new StepShader
            {
                Name = "Step create shadow",
                StepShaders = new[]
                {
                    new ShadowShader(GetShader("Shadow"))
                    {
                        ShadowColour = Color4.Red,
                        ShadowOffset = new Vector2(30, 30),
                    },
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
}
