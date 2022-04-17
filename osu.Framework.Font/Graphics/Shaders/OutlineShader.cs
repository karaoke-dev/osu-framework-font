// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.OpenGL.Buffers;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Shaders
{
    public class OutlineShader : InternalShader
    {
        public override string ShaderName => "Outline";

        public int Radius { get; set; }

        public Color4 OutlineColour { get; set; }

        public override void ApplyValue(FrameBuffer current)
        {
            var radius = Radius;
            GetUniform<int>(@"g_Radius").UpdateValue(ref radius);

            var outlineColourMatrix = new Vector4(OutlineColour.R, OutlineColour.G, OutlineColour.B, OutlineColour.A);
            GetUniform<Vector4>(@"g_OutlineColour").UpdateValue(ref outlineColourMatrix);

            var size = current.Size;
            GetUniform<Vector2>(@"g_TexSize").UpdateValue(ref size);
        }
    }
}
