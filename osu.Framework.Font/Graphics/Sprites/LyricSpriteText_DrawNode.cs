// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Buffers;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osu.Framework.Layout;
using osuTK;
using osuTK.Graphics;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Sprites
{
    public partial class LyricSpriteText
    {
        /// <summary>
        /// In order to signal the draw thread to re-draw the buffered container we version it.
        /// Our own version (update) keeps track of which version we are on, whereas the
        /// drawVersion keeps track of the version the draw thread is on.
        /// When forcing a redraw we increment updateVersion, pass it into each new drawnode
        /// and the draw thread will realize its drawVersion is lagging behind, thus redrawing.
        /// </summary>
        private long updateVersion;

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            var result = base.OnInvalidate(invalidation, source);

            if ((invalidation & Invalidation.DrawNode) > 0)
            {
                ++updateVersion;
                result = true;
            }

            return result;
        }

        // todo: should have a better way to let user able to customize formats?
        protected override DrawNode CreateDrawNode() => new LyricSpriteTextShaderEffectDrawNode(this, new LyricSpriteTextShaderEffectDrawNodeSharedData(null, false));

        /// <summary>
        /// <see cref="BufferedDrawNode"/> to apply <see cref="IShader"/>.
        /// </summary>
        protected class LyricSpriteTextShaderEffectDrawNode : BufferedDrawNode
        {
            protected new LyricSpriteText Source => (LyricSpriteText)base.Source;

            private long updateVersion;

            private IShader shader;

            public LyricSpriteTextShaderEffectDrawNode(LyricSpriteText source, LyricSpriteTextShaderEffectDrawNodeSharedData sharedData)
                : base(source, new LyricSpriteTextDrawNode(source), sharedData)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                updateVersion = Source.updateVersion;
                shader = Source.Shader;
            }

            protected override long GetDrawVersion() => updateVersion;

            protected override void PopulateContents()
            {
                base.PopulateContents();
                drawFrameBuffer();
            }

            protected override void DrawContents()
            {
                DrawFrameBuffer(SharedData.CurrentEffectBuffer, DrawRectangle, Color4.White);
            }

            private void drawFrameBuffer()
            {
                if (shader == null)
                    return;

                FrameBuffer current = SharedData.CurrentEffectBuffer;
                FrameBuffer target = SharedData.GetNextEffectBuffer();

                GLWrapper.SetBlend(BlendingParameters.None);

                using (BindFrameBuffer(target))
                {
                    UpdateUniforms(shader, current);

                    shader.Bind();
                    DrawFrameBuffer(current, new RectangleF(0, 0, current.Texture.Width, current.Texture.Height), ColourInfo.SingleColour(Color4.White));
                    shader.Unbind();
                }
            }

            protected virtual void UpdateUniforms(IShader targetShader, FrameBuffer current)
            {
                if (targetShader is ICustomizedShader customizedShader)
                    customizedShader.ApplyValue(current);
            }
        }

        public class LyricSpriteTextShaderEffectDrawNodeSharedData : BufferedDrawNodeSharedData
        {
            public LyricSpriteTextShaderEffectDrawNodeSharedData(RenderbufferInternalFormat[] formats, bool pixelSnapping)
                : base(2, formats, pixelSnapping)
            {
            }
        }

        /// <summary>
        /// <see cref="TexturedShaderDrawNode"/> to render characters in <see cref="LyricSpriteText"/>.
        /// </summary>
        internal class LyricSpriteTextDrawNode : TexturedShaderDrawNode
        {
            protected new LyricSpriteText Source => (LyricSpriteText)base.Source;

            private bool shadow;
            private ColourInfo shadowColour;
            private Vector2 shadowOffset;

            private readonly List<ScreenSpaceCharacterPart> parts = new List<ScreenSpaceCharacterPart>();

            public LyricSpriteTextDrawNode(LyricSpriteText source)
                : base(source)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                parts.Clear();
                parts.AddRange(Source.screenSpaceCharacters);
                shadow = Source.Shadow;

                if (shadow)
                {
                    shadowColour = Source.ShadowColour;
                    shadowOffset = Source.premultipliedShadowOffset;
                }
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);

                Shader.Bind();

                var avgColour = (Color4)DrawColourInfo.Colour.AverageColour;
                float shadowAlpha = MathF.Pow(Math.Max(Math.Max(avgColour.R, avgColour.G), avgColour.B), 2);

                //adjust shadow alpha based on highest component intensity to avoid muddy display of darker text.
                //squared result for quadratic fall-off seems to give the best result.
                var finalShadowColour = DrawColourInfo.Colour;
                finalShadowColour.ApplyChild(shadowColour.MultiplyAlpha(shadowAlpha));

                for (int i = 0; i < parts.Count; i++)
                {
                    if (shadow)
                    {
                        var shadowQuad = parts[i].DrawQuad;

                        DrawQuad(parts[i].Texture,
                            new Quad(
                                shadowQuad.TopLeft + shadowOffset,
                                shadowQuad.TopRight + shadowOffset,
                                shadowQuad.BottomLeft + shadowOffset,
                                shadowQuad.BottomRight + shadowOffset),
                            finalShadowColour, vertexAction: vertexAction, inflationPercentage: parts[i].InflationPercentage);
                    }

                    DrawQuad(parts[i].Texture, parts[i].DrawQuad, DrawColourInfo.Colour, vertexAction: vertexAction, inflationPercentage: parts[i].InflationPercentage);
                }

                Shader.Unbind();
            }
        }

        /// <summary>
        /// A character of a <see cref="LyricSpriteText"/> provided with screen space draw coordinates.
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
