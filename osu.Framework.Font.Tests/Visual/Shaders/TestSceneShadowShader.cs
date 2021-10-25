// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Font.Tests.Visual.Shaders
{
    public class TestSceneShadowShader : TestSceneInternalShader
    {
        [TestCase("#FFFF00", "(10,10)")]
        [TestCase("#FF0000", "(-20,-20)")]
        public void TestProperty(string colour, string offset)
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
    }
}
