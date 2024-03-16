// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual;

public abstract partial class BackgroundGridTestScene : FrameworkTestScene
{
    protected const int GRID_SIZE = 30;

    private const int column = 12;
    private const int row = 6;
    private const int spacing = 5;

    protected const string RED = "#AF0000";
    protected const string GREEN = "#00AF00";
    protected const string BLUE = "#0000AF";

    [Resolved]
    private ShaderManager shaderManager { get; set; } = null!;

    private readonly Container content;

    protected override Container<Drawable> Content => content;

    protected BackgroundGridTestScene()
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
                    }).ToArray(),
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
                    Colour = Color4Extensions.FromHex(GREEN),
                },
            },
        });
    }

    protected T GetShaderByType<T>() where T : InternalShader, new()
        => shaderManager.LocalInternalShader<T>();

    protected partial class DraggableCircle : Circle
    {
        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override void OnDrag(DragEvent e)
            => Position += e.Delta;
    }
}
