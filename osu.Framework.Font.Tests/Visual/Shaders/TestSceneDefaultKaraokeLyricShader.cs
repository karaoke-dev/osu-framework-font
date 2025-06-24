// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shaders;
using osuTK;

namespace osu.Framework.Font.Tests.Visual.Shaders
{
    public class TestSceneDefaultKaraokeLyricShader : TestSceneInternalShader
    {
        [TestCase("#FFFFFF", 10, "#0000FF")] // White text with blue outline
        [TestCase("#FF0000", 5, "#FFFFFF")] // Red text with white outline
        [TestCase("#FF0000", 0, "#FFFFFF")] // Red text with no outline
        public void TestOutline(string colour, int radius, string outlineColour)
        {
            AddStep("Apply shader", () =>
            {
                ShaderContainer.Shaders = new[]
                {
                    GetShaderByType<DefaultKaraokeLyricShader>().With(s =>
                    {
                        s.Colour = Color4Extensions.FromHex(colour);
                        s.Radius = radius;
                        s.OutlineColour = Color4Extensions.FromHex(outlineColour);

                        s.ShadowSigma = 200;
                        s.ShadowOffset = new Vector2(0, 1);
                    })
                };
            });
        }
    }
}
