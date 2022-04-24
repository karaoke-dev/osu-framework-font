// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Font.Tests.Visual.Containers
{
    public class TestSceneMaskingContainer : BackgroundGridTestSample
    {
        private readonly MaskingContainer<Drawable> maskingContainer;

        public TestSceneMaskingContainer()
        {
            Child = maskingContainer = new MaskingContainer<Drawable>
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(200),
                Children = new Drawable[]
                {
                    new Box
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(250),
                        Colour = Colour4.Red
                    },
                    new DraggableCircle
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Colour = Color4.Blue,
                        Size = new Vector2(100)
                    }
                }
            };
        }

        [TestCase(Edges.None)]
        [TestCase(Edges.Left)]
        [TestCase(Edges.Right)]
        [TestCase(Edges.Top)]
        [TestCase(Edges.Bottom)]
        [TestCase(Edges.Vertical)]
        [TestCase(Edges.Horizontal)]
        [TestCase(Edges.Left | Edges.Top)]
        public void TestMasking(Edges edges)
        {
            AddStep("apply masking", () =>
            {
                maskingContainer.MaskingEdges = edges;
            });
        }
    }
}
