// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osu.Framework.Tests.Helper;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Tests.Visual.Sprites
{
    public class TestSceneLyricSpriteTextWithShader : TestScene
    {
        [Resolved]
        private ShaderManager shaderManager { get; set; }

        [TestCase("CRT")]
        [TestCase("not_found_shader")] // notice that missing shader will let whole sprite text being white.
        public void ApplyShader(string shaderName)
        {
            var shader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, shaderName);
            AddStep("Create lyric", () => setContents(() => new LyricSpriteText
            {
                Text = "カラオケ",
                Rubies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }),
                Romajies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }),
                Shader = shader,
            }));
        }

        private void setContents(Func<LyricSpriteText> creationFunction)
        {
            Child = new Container
            {
                Size = new Vector2(200, 100),
                Children = new Drawable[]
                {
                    new Box
                    {
                        Colour = Color4.CornflowerBlue,
                        RelativeSizeAxes = Axes.Both,
                    },
                    creationFunction()
                }
            };
        }
    }
}
