﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
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

            private long updateVersion;

            public KaraokeSpriteTextShaderEffectDrawNode(KaraokeSpriteText<T> source, MultiShaderBufferedDrawNodeSharedData sharedData)
                : base(source, new CompositeDrawableDrawNode(source), sharedData)
            {
            }

            public override void ApplyState()
            {
                base.ApplyState();

                // todo: figure out why this works if use invalidation instead of version.
                updateVersion = Source.InvalidationID;
            }

            protected override long GetDrawVersion() => updateVersion;

            public List<DrawNode> Children
            {
                get => Child.Children;
                set => Child.Children = value;
            }

            public bool AddChildDrawNodes => RequiresRedraw;
        }
    }
}