// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Font.Tests.Visual.Shaders;

public partial class TestSceneRainbowShader : CustomizedShaderTestScene
{
    [TestCase("(0,1)", 1, 1, 1, 1, 1)] // normal state.
    [TestCase("(0.5,0.7)", 1, 1, 1, 1, 1)] // restrict color range?
    [TestCase("(0,1)", 3, 1, 1, 1, 1)] // runs faster.
    [TestCase("(0,1)", 1, 0.5f, 1, 1, 1)] // looks lighter
    [TestCase("(0,1)", 1, 1, 0.5f, 1, 1)] // looks darker.
    [TestCase("(0,1)", 1, 1, 1, 0.1f, 1)] // see the whole color range or only 0.n at the current time.
    [TestCase("(0,1)", 1, 1, 1, 1, 0.5f)] // mix with origin value.
    public void TestProperty(string uv, float speed, float saturation, float brightness, float section, float mix)
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
}
