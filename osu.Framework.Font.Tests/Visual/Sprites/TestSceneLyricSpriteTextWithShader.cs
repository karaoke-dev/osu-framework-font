// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.OpenGL.Buffers;
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

        [TestCase]
        public void ApplyShaderWithParams()
        {
            var shader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, "Outline");
            var outlineShader = new OutlineShader(shader);

            AddStep("Create lyric", () => setContents(() => new LyricSpriteText
            {
                Text = "カラオケ",
                Rubies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }),
                Romajies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }),
                Shader = outlineShader,
            }));
        }

        private void setContents(Func<LyricSpriteText> creationFunction)
        {
            Child = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Box
                    {
                        Colour = Color4.CornflowerBlue,
                        RelativeSizeAxes = Axes.Both,
                    },
                    creationFunction().With(x => x.Scale = new Vector2(5)),
                }
            };
        }

        private class OutlineShader : CustomizedShader
        {
            public OutlineShader(IShader originShader)
                : base(originShader)
            {
            }

            public override void ApplyValue(FrameBuffer current)
            {
                var radius = 10;
                GetUniform<int>(@"g_Radius").UpdateValue(ref radius);

                var colour = Color4.Green;
                var colourMatrix = new Vector4(colour.R, colour.G, colour.B, colour.A);
                GetUniform<Vector4>(@"g_Colour").UpdateValue(ref colourMatrix);

                var size = current.Size;
                GetUniform<Vector2>(@"g_TexSize").UpdateValue(ref size);
            }
        }
    }
}
