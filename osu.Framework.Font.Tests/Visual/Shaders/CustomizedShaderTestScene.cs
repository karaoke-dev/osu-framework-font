// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace osu.Framework.Font.Tests.Visual.Shaders
{
    public class CustomizedShaderTestScene : ShaderTestScene
    {
        [Resolved]
        private TextureStore textures { get; set; }

        [TestCase(0, "#FF0000")]
        [TestCase(10, "#00FF00")]
        [TestCase(20, "#0000FF")]
        [TestCase(100, "#0000FF")] // it might cause performance issue if set radius too large.
        public void TestOutlineShader(int radius, string colour)
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

        [TestCase("#FFFF00", "(10,10)")]
        [TestCase("#FF0000", "(-20,-20)")]
        public void TestShadowShader(string colour, string offset)
        {
            AddStep("Apply shader", () =>
            {
                ShaderContainer.Shaders = new[]
                {
                    GetShaderByType<ShadowShader>().With(s =>
                    {
                        s.ShadowColour = Color4Extensions.FromHex(colour);
                        s.ShadowOffset = TestCaseVectorHelper.ParseVector2(offset);
                    })
                };
            });
        }

        [TestCase("(0,1)", 1, 1, 1, 1, 1)] // normal state.
        [TestCase("(0.5,0.7)", 1, 1, 1, 1, 1)] // restrict color range?
        [TestCase("(0,1)", 3, 1, 1, 1, 1)] // runs faster.
        [TestCase("(0,1)", 1, 0.5f, 1, 1, 1)] // looks lighter
        [TestCase("(0,1)", 1, 1, 0.5f, 1, 1)] // looks darker.
        [TestCase("(0,1)", 1, 1, 1, 0.1f, 1)] // see the whole color range or only 0.n at the current time.
        [TestCase("(0,1)", 1, 1, 1, 1, 0.5f)] // mix with origin value.
        public void TestRainbowShader(string uv, float speed, float saturation, float brightness, float section, float mix)
        {
            AddStep("Apply shader", () =>
            {
                ShaderContainer.Shaders = new[]
                {
                    GetShaderByType<RainbowShader>().With(s =>
                    {
                        s.Uv = TestCaseVectorHelper.ParseVector2(uv);
                        s.Speed = speed;
                        s.Saturation = saturation;
                        s.Brightness = brightness;
                        s.Section = section;
                        s.Mix = mix;
                    })
                };
            });
        }

        [TestCase(5, 5)]
        [TestCase(5, 20)]
        [TestCase(20, 20)]
        public void TestPixelShader(float x, float y)
        {
            AddStep("Apply shader", () =>
            {
                ShaderContainer.Shaders = new[]
                {
                    GetShaderByType<PixelShader>().With(s =>
                    {
                        s.Size = new Vector2(x, y);
                    })
                };
            });
        }

        [TestCase("sample-texture", 1f, 1f)]
        [TestCase("sample-texture", 5, 20)]
        [TestCase("sample-texture", 20, 20)]
        public void TestRepeatMovingBackgroundShader(string textureName, float width, float height)
        {
            AddStep("Apply shader", () =>
            {
                ShaderContainer.Shaders = new[]
                {
                    GetShaderByType<RepeatMovingBackgroundShader>().With(s =>
                    {
                        s.Texture = textures.Get(textureName);
                        s.TextureSize = new Vector2(width, height);
                    })
                };
            });
        }
    }
}
