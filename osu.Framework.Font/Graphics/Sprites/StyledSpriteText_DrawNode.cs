// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Sprites
{
    public partial class StyledSpriteText
    {
        internal class SpriteTextDrawNode : TexturedShaderDrawNode
        {
            protected new StyledSpriteText Source => (StyledSpriteText)base.Source;

            private bool shadow;
            private ColourInfo shadowColour;
            private Vector2 shadowOffset;

            private bool outline;
            private int outlineRadius;
            private float outlineSigma = 10;

            private IShader outlineShader;

            private readonly List<ScreenSpaceCharacterPart> parts = new List<ScreenSpaceCharacterPart>();

            public SpriteTextDrawNode(StyledSpriteText source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                parts.Clear();
                parts.AddRange(Source.screenSpaceCharacters);
                shadow = Source.Shadow;
                outline = Source.Outline;

                if (shadow)
                {
                    shadowColour = Source.ShadowColour;
                    shadowOffset = Source.premultipliedShadowOffset;
                }

                if (outline)
                {
                    outlineRadius = (int)Source.outlineRadius;
                    outlineShader = Source.outlineShader;
                }
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);

                outlineSigma = 100;


                var avgColour = (Color4)DrawColourInfo.Colour.AverageColour;
                float shadowAlpha = MathF.Pow(Math.Max(Math.Max(avgColour.R, avgColour.G), avgColour.B), 2);

                //adjust shadow alpha based on highest component intensity to avoid muddy display of darker text.
                //squared result for quadratic fall-off seems to give the best result.
                var finalShadowColour = DrawColourInfo.Colour;
                finalShadowColour.ApplyChild(shadowColour.MultiplyAlpha(shadowAlpha));

                Shader.Bind();

                foreach (var current in parts)
                {
                    if (shadow)
                    {
                        var shadowQuad = current.DrawQuad;

                        DrawQuad(current.Texture,
                            new Quad(
                                shadowQuad.TopLeft + shadowOffset,
                                shadowQuad.TopRight + shadowOffset,
                                shadowQuad.BottomLeft + shadowOffset,
                                shadowQuad.BottomRight + shadowOffset),
                            finalShadowColour, vertexAction: vertexAction, inflationPercentage: current.InflationPercentage);
                    }

                    DrawQuad(current.Texture, current.DrawQuad, DrawColourInfo.Colour, vertexAction: vertexAction, inflationPercentage: current.InflationPercentage);
                }
                
                Shader.Unbind();

                if (outline)
                {
                    outlineShader.Bind();

                    foreach (var current in parts)
                    {
                        /*
                        outlineShader.GetUniform<int>(@"g_Radius").UpdateValue(ref outlineRadius);
                        outlineShader.GetUniform<float>(@"g_Sigma").UpdateValue(ref outlineSigma);

                        Vector2 size = current.DrawQuad.Size;
                        outlineShader.GetUniform<Vector2>(@"g_TexSize").UpdateValue(ref size);

                        float radians = -MathUtils.DegreesToRadians(0);
                        Vector2 blur = new Vector2(-0.1f, -0.1f);
                        outlineShader.GetUniform<Vector2>(@"g_BlurDirection").UpdateValue(ref blur);
                        */

                        DrawQuad(current.Texture, current.DrawQuad, Color4.Blue, vertexAction: vertexAction, inflationPercentage: current.InflationPercentage);
                    }

                    outlineShader.Unbind();
                }
            }
        }

        /// <summary>
        /// A character of a <see cref="StyledSpriteText"/> provided with screen space draw coordinates.
        /// </summary>
        internal struct ScreenSpaceCharacterPart
        {
            /// <summary>
            /// The screen-space quad for the character to be drawn in.
            /// </summary>
            public Quad DrawQuad;

            /// <summary>
            /// Extra padding for the character's texture.
            /// </summary>
            public Vector2 InflationPercentage;

            /// <summary>
            /// The texture to draw the character with.
            /// </summary>
            public Texture Texture;
        }
    }
}
