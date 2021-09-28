// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.OpenGL.Buffers;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Shaders
{
    public class OutlineShader : CustomizedShader
    {
        public const string SHADER_NAME = "Outline";
        public int Radius { get; set; }

        public Color4 OutlineColour { get; set; }

        public OutlineShader(IShader originShader)
            : base(originShader)
        {
        }

        public override void ApplyValue(FrameBuffer current)
        {
            var radius = Radius;
            GetUniform<int>(@"g_Radius").UpdateValue(ref radius);

            var colourMatrix = new Vector4(OutlineColour.R, OutlineColour.G, OutlineColour.B, OutlineColour.A);
            GetUniform<Vector4>(@"g_Colour").UpdateValue(ref colourMatrix);

            var size = current.Size;
            GetUniform<Vector2>(@"g_TexSize").UpdateValue(ref size);
        }
    }
}
