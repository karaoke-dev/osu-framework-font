// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Font.Tests.Visual.Shaders
{
    public class TestSceneDefaultKaraokeLyricShader : TestSceneInternalShader
    {
        [TestCase("#FFFF00", 10, "#FFFF00")]
        [TestCase("#FF0000", 20, "#FFFFFF")]
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
                    })
                };
            });
        }
    }
}
