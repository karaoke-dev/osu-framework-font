// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Tests.Visual.Sprites
{
    public class TestSceneLyricTexture : TestScene
    {
        public TestSceneLyricTexture()
        {
            AddStep("Test SolidTexture", () => Child = createKaroakeText(new SolidTexture { SolidColor = Color4.Blue }));
            AddStep("Test Mixedexture(Gradient)", () => Child = createKaroakeText(new Mixedexture
            {
                Colors = new Dictionary<float, SRGBColour>
                {
                    { 0, Color4.Red },
                    { 0.5f, Color4.Red },
                    { 0.6f, Color4.Blue },
                    { 1, Color4.Blue }
                },
                Type = Mixedexture.MixedType.Gradient
            }));
            AddStep("Test Mixedexture(MilleFeuille)", () => Child = createKaroakeText(new Mixedexture
            {
                Colors = new Dictionary<float, SRGBColour>
                {
                    { 0, Color4.Red },
                    { 0.4f, Color4.Red },
                    { 0.6f, Color4.Blue },
                    { 1, Color4.Blue }
                },
                Type = Mixedexture.MixedType.MilleFeuille
            }));
        }

        private Drawable createKaroakeText(ILyricTexture lyricTexture)
        {
            return new Sprite
            {
                Margin = new MarginPadding(30),
                Texture = lyricTexture.CreateTexture(100, 80, Direction.Vertical),
                Scale = new Vector2(3)
            };
        }
    }
}
