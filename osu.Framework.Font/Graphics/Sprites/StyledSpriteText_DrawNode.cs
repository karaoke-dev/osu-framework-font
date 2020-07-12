// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Textures;
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
            private Vector4 shadowOutlineColour;
            private Colour4 shadowColour;
            private Vector2 shadowOffset;

            private bool outline;
            private Colour4 outlineColour;
            private float outlineRadius;

            private Vector4 textColour;

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

                var color4 = DrawColourInfo.Colour.AverageColour.Linear;
                textColour = new Vector4(color4.R, color4.G, color4.B, color4.A);

                if (shadow)
                {
                    shadowColour = Source.ShadowColour;
                    shadowOutlineColour = new Vector4(Source.ShadowColour.R, Source.ShadowColour.G, Source.ShadowColour.B, Source.ShadowColour.A);
                    shadowOffset = Source.premultipliedShadowOffset;
                }

                if (outline)
                {
                    outlineColour = Source.OutlineColour;
                    outlineRadius = Source.outlineRadius / 512;
                }
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);

                var avgColour = (Color4)DrawColourInfo.Colour.AverageColour;
                float shadowAlpha = MathF.Pow(Math.Max(Math.Max(avgColour.R, avgColour.G), avgColour.B), 2);

                //adjust shadow alpha based on highest component intensity to avoid muddy display of darker text.
                //squared result for quadratic fall-off seems to give the best result.
                //var finalShadowColour = DrawColourInfo.Colour;
                //finalShadowColour.ApplyChild(shadowColour.MultiplyAlpha(shadowAlpha));

                Shader.Bind();
                Shader.GetUniform<float>(@"g_outlineRadius").UpdateValue(ref outlineRadius);

                foreach (var current in parts)
                {
                    if (shadow)
                    {
                        Shader.GetUniform<Vector4>(@"g_outlineColour").UpdateValue(ref shadowOutlineColour);
                        var shadowQuad = current.DrawQuad;

                        DrawQuad(current.Texture,
                            new Quad(
                                shadowQuad.TopLeft + shadowOffset,
                                shadowQuad.TopRight + shadowOffset,
                                shadowQuad.BottomLeft + shadowOffset,
                                shadowQuad.BottomRight + shadowOffset),
                            shadowColour, vertexAction: vertexAction, inflationPercentage: current.InflationPercentage);
                    }

                    Shader.GetUniform<Vector4>(@"g_outlineColour").UpdateValue(ref textColour);

                    DrawQuad(current.Texture, current.DrawQuad, outlineColour, vertexAction: vertexAction, inflationPercentage: current.InflationPercentage);
                }
                
                Shader.Unbind();
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
