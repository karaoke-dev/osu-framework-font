// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.OpenGL.Buffers;
using osuTK;

namespace osu.Framework.Graphics.Shaders
{
    public class PixelShader : InternalShader
    {
        public override string ShaderName => "Pixel";

        public Vector2 Size { get; set; } = new Vector2(5);

        public override void ApplyValue(FrameBuffer current)
        {
            var size = Size;
            GetUniform<Vector2>(@"g_Size").UpdateValue(ref size);

            var textureSize = current.Size;
            GetUniform<Vector2>(@"g_TexSize").UpdateValue(ref textureSize);
        }
    }
}
