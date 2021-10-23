// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.OpenGL.Buffers;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Shaders
{
    public class ShadowShader : InternalShader
    {
        public override string ShaderName => "Shadow";

        public Color4 ShadowColour { get; set; }

        public Vector2 ShadowOffset { get; set; }

        public ShadowShader(IShader originShader)
            : base(originShader)
        {
        }

        public override void ApplyValue(FrameBuffer current)
        {
            var shadowColour = new Vector4(ShadowColour.R, ShadowColour.G, ShadowColour.B, ShadowColour.A);
            GetUniform<Vector4>(@"g_Colour").UpdateValue(ref shadowColour);

            var shadowOffset = new Vector2(-ShadowOffset.X, ShadowOffset.Y);
            GetUniform<Vector2>(@"g_Offset").UpdateValue(ref shadowOffset);

            var size = current.Size;
            GetUniform<Vector2>(@"g_TexSize").UpdateValue(ref size);
        }
    }
}
