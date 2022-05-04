// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Font.Tests.Visual.Shaders
{
    public class TestSceneShadowShader : InternalShaderTestScene
    {
        [TestCase(RED, "(3,3)")]
        [TestCase(GREEN, "(-4,-4)")]
        [TestCase(BLUE, "(0,0)")]
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
