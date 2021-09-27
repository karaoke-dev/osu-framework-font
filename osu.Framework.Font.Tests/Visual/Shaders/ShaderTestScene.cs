// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Shaders
{
    public abstract class ShaderTestScene : TestScene
    {
        private const int column = 12;
        private const int row = 6;
        private const int spacing = 5;
        private const int grid_size = 30;

        [Resolved]
        private ShaderManager shaderManager { get; set; }

        protected readonly TestShaderContainer ShaderContainer;

        protected ShaderTestScene()
        {
            const int x = (grid_size + spacing) * column - spacing;
            const int y = (grid_size + spacing) * row - spacing;

            Children = new Drawable[]
            {
                new FillFlowContainer<Box>
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(x, y),
                    Name = "Background",
                    Spacing = new Vector2(spacing),
                    Children = Enumerable.Range(0, column * row).Select(_ => new Box
                    {
                        Colour = Color4.DarkBlue,
                        Size = new Vector2(grid_size),
                    }).ToArray()
                },
                new DraggableCircle
                {
                    Size = new Vector2(50),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = Color4.Purple,
                },
                ShaderContainer = new TestShaderContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(x, y),
                    Children = new Drawable[]
                    {
                        new DraggableBox
                        {
                            X = -100,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Size = new Vector2(50),
                            Colour = Color4.Red,
                        },
                        new DraggableCircle
                        {
                            Size = new Vector2(50),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Colour = Color4.Blue,
                        },
                        new DraggableTriangle
                        {
                            X = 100,
                            Size = new Vector2(50),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Colour = Color4.Green,
                        },
                        createBorderBox("Left up cube", Anchor.TopLeft),
                        createBorderBox("Right up cube", Anchor.TopRight),
                        createBorderBox("Left down cube", Anchor.BottomLeft),
                        createBorderBox("Right down cube", Anchor.BottomRight),
                    }
                }
            };

            static Box createBorderBox(string name, Anchor position)
                => new()
                {
                    Name = name,
                    Anchor = position,
                    Origin = position,
                    Colour = Color4.Red,
                    Size = new Vector2(grid_size),
                };
        }

        protected IShader GetShader(string shaderName)
            => shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, shaderName);

        private class DraggableBox : Box
        {
            protected override bool OnDragStart(DragStartEvent e) => true;

            protected override void OnDrag(DragEvent e)
                => Position += e.Delta;
        }

        private class DraggableCircle : Circle
        {
            protected override bool OnDragStart(DragStartEvent e) => true;

            protected override void OnDrag(DragEvent e)
                => Position += e.Delta;
        }

        private class DraggableTriangle : Triangle
        {
            protected override bool OnDragStart(DragStartEvent e) => true;

            protected override void OnDrag(DragEvent e)
                => Position += e.Delta;
        }

        protected class TestShaderContainer : Container, IMultiShaderBufferedDrawable
        {
            public IShader TextureShader { get; private set; }
            public IShader RoundedTextureShader { get; private set; }

            private readonly List<IShader> shaders = new();

            public IReadOnlyList<IShader> Shaders
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
            private void load(ShaderManager shaders)
            {
                TextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
                RoundedTextureShader = shaders.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);
            }

            // todo: should have a better way to let user able to customize formats?
            protected override DrawNode CreateDrawNode()
                => new TestShaderContainerShaderEffectDrawNode(this, new MultiShaderBufferedDrawNodeSharedData());

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
}
