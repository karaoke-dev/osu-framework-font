// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Extensions;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Graphics.Sprites
{
    public partial class KaraokeSpriteText<T>
    {
        protected override Quad ComputeScreenSpaceDrawQuad()
        {
            // make draw size become bigger (for not masking the shader).
            var newRectangle = DrawRectangle.Scale(2);
            return ToScreenSpace(newRectangle);
        }

        protected override DrawNode CreateDrawNode()
            => new KaraokeSpriteTextShaderEffectDrawNode(this, sharedData);

        /// <summary>
        /// <see cref="BufferedDrawNode"/> to apply <see cref="IShader"/>.
        /// </summary>
        protected class KaraokeSpriteTextShaderEffectDrawNode : MultiShaderBufferedDrawNode, ICompositeDrawNode
        {
            protected new KaraokeSpriteText<T> Source => (KaraokeSpriteText<T>)base.Source;

            protected new CompositeDrawableDrawNode Child => (CompositeDrawableDrawNode)base.Child;

            private IShader[] leftLyricShaders;
            private IShader[] rightLyricShaders;

            public KaraokeSpriteTextShaderEffectDrawNode(KaraokeSpriteText<T> source, MultiShaderBufferedDrawNodeSharedData sharedData)
                : base(source, new CompositeDrawableDrawNode(source), sharedData)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                leftLyricShaders = Source.LeftLyricTextShaders.ToArray();
                rightLyricShaders = Source.RightLyricTextShaders.ToArray();
            }

            protected override long GetDrawVersion()
            {
                // if contains shader that need to apply time, then need to force run populate contents in each frame.
                if (ContainTimePropertyShader(leftLyricShaders) || ContainTimePropertyShader(rightLyricShaders))
                {
                    ResetDrawVersion();
                }

                return base.GetDrawVersion();
            }

            public List<DrawNode> Children
            {
                get => Child.Children;
                set => Child.Children = value;
            }

            public bool AddChildDrawNodes => RequiresRedraw;
        }
    }
}
