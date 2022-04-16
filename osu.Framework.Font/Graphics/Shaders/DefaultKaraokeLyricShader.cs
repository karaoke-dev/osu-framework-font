// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.OpenGL.Buffers;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Shaders
{
    public class DefaultKaraokeLyricShader : InternalShader
    {
        public override string ShaderName => "DefaultKaraokeFont";

        public Color4 Colour { get; set; }

        public int Radius { get; set; }

        public Color4 OutlineColour { get; set; }

        public Vector2 ShaderOffset { get; set; }

        public int ShaderSize { get; set; }

        public override void ApplyValue(FrameBuffer current)
        {
            var radius = Radius;
            GetUniform<int>(@"g_Radius").UpdateValue(ref radius);

            var colourMatrix = new Vector4(Colour.R, Colour.G, Colour.B, Colour.A);
            GetUniform<Vector4>(@"g_Colour").UpdateValue(ref colourMatrix);

            var outlineColourMatrix = new Vector4(OutlineColour.R, OutlineColour.G, OutlineColour.B, OutlineColour.A);
            GetUniform<Vector4>(@"g_OutlineColour").UpdateValue(ref outlineColourMatrix);

            var size = current.Size;
            GetUniform<Vector2>(@"g_TexSize").UpdateValue(ref size);
        }
    }
}
