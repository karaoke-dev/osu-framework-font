// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.OpenGL.Buffers;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics.Shaders
{
    public class RepeatMovingBackgroundShader : InternalShader
    {
        public override string ShaderName => "RepeatMovingBackground";

        public Texture Texture { get; set; }

        public Vector2 Repeat { get; set; } = new Vector2(3);

        public RepeatMovingBackgroundShader(IShader originShader)
            : base(originShader)
        {
        }

        public override void ApplyValue(FrameBuffer current)
        {
            Texture?.TextureGL.Bind(TextureUnit.Texture3);

            var unitId = TextureUnit.Texture3 - TextureUnit.Texture0;
            GetUniform<int>(@"g_RepeatSample").UpdateValue(ref unitId);

            var repeat = Repeat;
            GetUniform<Vector2>("g_Repeat").UpdateValue(ref repeat);
        }
    }
}
