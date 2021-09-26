// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics.OpenGL.Vertices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;
using osu.Framework.Layout;
using osuTK;

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
        protected override DrawNode CreateDrawNode()
            => new LyricSpriteTextShaderEffectDrawNode(this, new MultiShaderBufferedDrawNodeSharedData());

        /// <summary>
        /// <see cref="BufferedDrawNode"/> to apply <see cref="IShader"/>.
        /// </summary>
        protected class LyricSpriteTextShaderEffectDrawNode : MultiShaderBufferedDrawNode
        {
            protected new LyricSpriteText Source => (LyricSpriteText)base.Source;

            private long updateVersion;

            public LyricSpriteTextShaderEffectDrawNode(LyricSpriteText source, MultiShaderBufferedDrawNodeSharedData sharedData)
                : base(source, new LyricSpriteTextDrawNode(source), sharedData)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                updateVersion = Source.updateVersion;
            }

            protected override long GetDrawVersion() => updateVersion;
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
