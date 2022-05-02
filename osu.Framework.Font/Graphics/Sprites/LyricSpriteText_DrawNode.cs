// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace osu.Framework.Graphics.Sprites
{
    public partial class LyricSpriteText
    {
        protected override Quad ComputeScreenSpaceDrawQuad()
        {
            // make draw size become bigger (for not masking the shader).
            var quad = ToScreenSpace(DrawRectangle);
            var drawRectangele = Shaders.OfType<IApplicableToDrawQuad>()
                                        .Select(x => x.ComputeScreenSpaceDrawQuad(quad).AABBFloat)
                                        .Aggregate(quad.AABBFloat, RectangleF.Union);

            return Quad.FromRectangle(drawRectangele);
        }

        protected override DrawNode CreateDrawNode()
            => new LyricSpriteTextShaderEffectDrawNode(this, sharedData);

        /// <summary>
        /// <see cref="BufferedDrawNode"/> to apply <see cref="IShader"/>.
        /// </summary>
        protected class LyricSpriteTextShaderEffectDrawNode : SingleShaderBufferedDrawNode
        {
            public LyricSpriteTextShaderEffectDrawNode(LyricSpriteText source, BufferedDrawNodeSharedData sharedData)
                : base(source, new LyricSpriteTextDrawNode(source), sharedData)
            {
            }
        }

        /// <summary>
        /// <see cref="TexturedShaderDrawNode"/> to render characters in <see cref="LyricSpriteText"/>.
        /// </summary>
        internal class LyricSpriteTextDrawNode : TexturedShaderDrawNode
        {
            protected new LyricSpriteText Source => (LyricSpriteText)base.Source;

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
            }

            public override void Draw(Action<TexturedVertex2D> vertexAction)
            {
                base.Draw(vertexAction);

                Shader.Bind();

                for (int i = 0; i < parts.Count; i++)
                {
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
