// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Graphics.Sprites
{
    public partial class KaraokeSpriteText<T>
    {
        // todo: should have a better way to let user able to customize formats?
        protected override DrawNode CreateDrawNode()
            => new KaraokeSpriteTextShaderEffectDrawNode(this, new MultiShaderBufferedDrawNodeSharedData());

        /// <summary>
        /// <see cref="BufferedDrawNode"/> to apply <see cref="IShader"/>.
        /// </summary>
        protected class KaraokeSpriteTextShaderEffectDrawNode : MultiShaderBufferedDrawNode, ICompositeDrawNode
        {
            protected new CompositeDrawableDrawNode Child => (CompositeDrawableDrawNode)base.Child;

            public KaraokeSpriteTextShaderEffectDrawNode(KaraokeSpriteText<T> source, MultiShaderBufferedDrawNodeSharedData sharedData)
                : base(source, new CompositeDrawableDrawNode(source), sharedData)
            {
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
