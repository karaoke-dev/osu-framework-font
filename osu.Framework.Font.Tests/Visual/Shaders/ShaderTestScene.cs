// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Shaders;

public abstract class ShaderTestScene : BackgroundGridTestScene
{
    protected readonly TestShaderContainer ShaderContainer;

    protected ShaderTestScene()
    {
        Child = ShaderContainer = new TestShaderContainer
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            RelativeSizeAxes = Axes.Both,
            Children = new Drawable[]
            {
                new DraggableBox
                {
                    X = -100,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(50),
                    Colour = Color4Extensions.FromHex(RED),
                },
                new DraggableCircle
                {
                    Size = new Vector2(50),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Color4Extensions.FromHex(BLUE),
                },
                new DraggableTriangle
                {
                    X = 100,
                    Size = new Vector2(50),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Color4Extensions.FromHex(GREEN)
                },
                new DraggableText
                {
                    X = -100,
                    Y = 50,
                    Text = "CA",
                    Font = FontUsage.Default.With(size: 72),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                createBorderBox("Left up cube", Anchor.TopLeft),
                createBorderBox("Right up cube", Anchor.TopRight),
                createBorderBox("Left down cube", Anchor.BottomLeft),
                createBorderBox("Right down cube", Anchor.BottomRight),
            }
        };

        static Box createBorderBox(string name, Anchor position)
            => new()
            {
                Name = name,
                Anchor = position,
                Origin = position,
                Colour = Color4Extensions.FromHex(RED),
                Size = new Vector2(GRID_SIZE),
            };
    }

    protected class DraggableBox : Box
    {
        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
            => Position += e.Delta;
    }

    protected class DraggableTriangle : Triangle
    {
        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
            => Position += e.Delta;
    }

    protected class DraggableText : SpriteText
    {
        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
            => Position += e.Delta;
    }

    protected class TestShaderContainer : Container, IMultiShaderBufferedDrawable
    {
        public IShader TextureShader { get; private set; } = null!;

        private readonly List<ICustomizedShader> shaders = new();

        // todo: should have a better way to let user able to customize formats?
        private readonly MultiShaderBufferedDrawNodeSharedData sharedData = new();

        public IReadOnlyList<ICustomizedShader> Shaders
        {
            get => shaders;
            set
            {
                shaders.Clear();
                shaders.AddRange(value);
                Invalidate(Invalidation.DrawNode);
            }
        }

        public Color4 BackgroundColour => new(0, 0, 0, 0);
        public DrawColourInfo? FrameBufferDrawColour => base.DrawColourInfo;
        public Vector2 FrameBufferScale => Vector2.One;

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaderManager)
        {
            TextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
        }

        protected override DrawNode CreateDrawNode()
            => new TestShaderContainerShaderEffectDrawNode(this, sharedData);

        /// <summary>
        /// <see cref="BufferedDrawNode"/> to apply <see cref="IShader"/>.
        /// </summary>
        protected class TestShaderContainerShaderEffectDrawNode : MultiShaderBufferedDrawNode, ICompositeDrawNode
        {
            protected new CompositeDrawableDrawNode Child => (CompositeDrawableDrawNode)base.Child;

            public TestShaderContainerShaderEffectDrawNode(TestShaderContainer source, MultiShaderBufferedDrawNodeSharedData sharedData)
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
