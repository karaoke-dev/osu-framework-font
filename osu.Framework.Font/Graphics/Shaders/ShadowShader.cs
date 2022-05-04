// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Primitives;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Shaders
{
    public class ShadowShader : InternalShader, IApplicableToDrawQuad, IHasTextureSize
    {
        public override string ShaderName => "Shadow";

        public Color4 ShadowColour { get; set; }

        public Vector2 ShadowOffset { get; set; }

        public override void ApplyValue()
        {
            var shadowColour = new Vector4(ShadowColour.R, ShadowColour.G, ShadowColour.B, ShadowColour.A);
            GetUniform<Vector4>(@"g_Colour").UpdateValue(ref shadowColour);

            var shadowOffset = new Vector2(-ShadowOffset.X, ShadowOffset.Y);
            GetUniform<Vector2>(@"g_Offset").UpdateValue(ref shadowOffset);
        }

        public Quad ComputeScreenSpaceDrawQuad(Quad originDrawQuad)
        {
            var rectangle = originDrawQuad.AABBFloat.Inflate(new MarginPadding
            {
                Left = Math.Max(-ShadowOffset.X, 0),
                Right = Math.Max(ShadowOffset.X, 0),
                Top = Math.Max(-ShadowOffset.Y, 0),
                Bottom = Math.Max(ShadowOffset.Y, 0),
            });

            return Quad.FromRectangle(rectangle);
        }
    }
}
