// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Sprites
{
    public class TestSceneKaraokeSpriteTextWithShader : TestScene
    {
        [Resolved]
        private ShaderManager shaderManager { get; set; }

        [TestCase("CRT")]
        [TestCase("not_found_shader")] // notice that missing shader will let whole sprite text being white.
        public void ApplyShader(string shaderName)
        {
            var shader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, shaderName);
            AddStep("Create lyric", () => setContents((spriteText) =>
            {
                spriteText.Shaders = new[]
                {
                    shader,
                };

                spriteText.Shadow = true;
                spriteText.ShadowOffset = new Vector2(3);
            }));
        }

        [TestCase("CRT", "CRT")]
        [TestCase("CRT", "not_found_shader")] // notice that missing shader will let whole sprite text being white.
        public void ApplyLyricTextShader(string leftShaderName, string rightShaderName)
        {
            var leftShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, leftShaderName);
            var rightShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, rightShaderName);
            AddStep("Create lyric", () => setContents((spriteText) =>
            {
                spriteText.LeftLyricTextShaders = new[]
                {
                    leftShader,
                };
                spriteText.RightLyricTextShaders = new[]
                {
                    rightShader,
                };

                spriteText.Shadow = true;
                spriteText.ShadowOffset = new Vector2(3);
            }));
        }

        [TestCase]
        public void ApplyLyricTextShaderWithParams()
        {
            var shader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, "Outline");
            AddStep("Create lyric", () => setContents((spriteText) =>
            {
                spriteText.LeftLyricTextShaders = new[]
                {
                    new OutlineShader(shader)
                    {
                        Radius = 10,
                        OutlineColour = Color4.Green,
                    },
                };
                spriteText.RightLyricTextShaders = new[]
                {
                    new OutlineShader(shader)
                    {
                        Radius = 10,
                        OutlineColour = Color4.Red,
                    },
                };
            }));
        }

        [TestCase]
        public void ApplyShaderInBothPart()
        {
            var outlineShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, "Outline");
            var crtShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, "CRT");
            AddStep("Create lyric", () => setContents((spriteText) =>
            {
                // apply shader in karaoke sprite text.
                spriteText.Shaders = new[]
                {
                    crtShader
                };

                // apply shader in lyric sprite text.
                spriteText.LeftLyricTextShaders = new[]
                {
                    new OutlineShader(outlineShader)
                    {
                        Radius = 10,
                        OutlineColour = Color4.Green,
                    },
                };
                spriteText.RightLyricTextShaders = new[]
                {
                    new OutlineShader(outlineShader)
                    {
                        Radius = 10,
                        OutlineColour = Color4.Red,
                    },
                };
            }));
        }

        private void setContents(Action<KaraokeSpriteText> applySpriteTextProperty)
        {
            var startTime = Time.Current;
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
                    new KaraokeSpriteText
                    {
                        Text = "カラオケ！",
                        Rubies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:か", "[1,2]:ら", "[2,3]:お", "[3,4]:け" }),
                        Romajies = TestCaseTagHelper.ParseParsePositionTexts(new[] { "[0,1]:ka", "[1,2]:ra", "[2,3]:o", "[3,4]:ke" }),
                        TimeTags = new Dictionary<TextIndex, double>
                        {
                            { new TextIndex(0), startTime + 500 },
                            { new TextIndex(1), startTime + 600 },
                            { new TextIndex(2), startTime + 1000 },
                            { new TextIndex(3), startTime + 1500 },
                            { new TextIndex(4), startTime + 2000 },
                        },
                        Scale = new Vector2(5)
                    }.With(applySpriteTextProperty),
                }
            };
        }
    }
}
