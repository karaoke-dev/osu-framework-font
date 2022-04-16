// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.OpenGL.Buffers;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Shaders
{
    public class DefaultKaraokeLyricShader : InternalShader
    {
        public override string ShaderName => "DefaultKaraokeLyric";

        public Color4 Colour { get; set; }

        public int Radius { get; set; }

        public Color4 OutlineColour { get; set; }

        public Vector2 ShadowOffset { get; set; }

        public int ShadowSize { get; set; }

        public int ShadowSigma { get; set; }

        public Vector2 ShadowColour { get; set; }

        public override void ApplyValue(FrameBuffer current)
        {
            // outline effect
            var colourMatrix = new Vector4(Colour.R, Colour.G, Colour.B, Colour.A);
            GetUniform<Vector4>(@"g_Colour").UpdateValue(ref colourMatrix);

            var radius = Radius;
            GetUniform<int>(@"g_Radius").UpdateValue(ref radius);

            var outlineColourMatrix = new Vector4(OutlineColour.R, OutlineColour.G, OutlineColour.B, OutlineColour.A);
            GetUniform<Vector4>(@"g_OutlineColour").UpdateValue(ref outlineColourMatrix);

            // shadow effect

            // common property.
            var size = current.Size;
            GetUniform<Vector2>(@"g_TexSize").UpdateValue(ref size);
        }
    }
}
