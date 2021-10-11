// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual
{
    public abstract class BackgroundGridTestSample : TestScene
    {
        protected const int GRID_SIZE = 30;

        private const int column = 12;
        private const int row = 6;
        private const int spacing = 5;

        [Resolved]
        private ShaderManager shaderManager { get; set; }

        private readonly Container content;

        protected override Container<Drawable> Content => content;

        protected BackgroundGridTestSample()
        {
            const int x = (GRID_SIZE + spacing) * column - spacing;
            const int y = (GRID_SIZE + spacing) * row - spacing;

            base.Content.Add(new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(x, y),
                Children = new Drawable[]
                {
                    new FillFlowContainer<Box>
                    {
                        Name = "Background",
                        RelativeSizeAxes = Axes.Both,
                        Spacing = new Vector2(spacing),
                        Children = Enumerable.Range(0, column * row).Select(_ => new Box
                        {
                            Colour = Color4.DarkBlue,
                            Size = new Vector2(GRID_SIZE),
                        }).ToArray()
                    },
                    new DraggableCircle
                    {
                        Name = "Test background change object",
                        Size = new Vector2(50),
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Color4.Purple,
                    },
                    content = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    new DraggableCircle
                    {
                        Name = "Test background change object",
                        Size = new Vector2(GRID_SIZE),
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        Colour = Color4.Green,
                    },
                }
            });
        }

        protected IShader GetShader(string shaderName)
            => shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, shaderName);

        protected T GetShaderByType<T>() where T : class, ICustomizedShader
            => shaderManager.LocalCustomizedShader<T>();

        protected class DraggableBox : Box
        {
            protected override bool OnDragStart(DragStartEvent e) => true;

            protected override void OnDrag(DragEvent e)
                => Position += e.Delta;
        }

        protected class DraggableCircle : Circle
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
    }
}
