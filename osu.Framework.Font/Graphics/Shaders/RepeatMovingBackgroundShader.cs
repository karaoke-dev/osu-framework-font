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

        public Vector2 TextureSize { get; set; } = new Vector2(10);

        public Vector2 Border { get; set; }

        public RepeatMovingBackgroundShader(IShader originShader)
            : base(originShader)
        {
        }

        public override void ApplyValue(FrameBuffer current)
        {
            if (Texture == null)
                return;

            Texture.TextureGL.Bind(TextureUnit.Texture1);

            var unitId = TextureUnit.Texture1 - TextureUnit.Texture0;
            GetUniform<int>(@"g_RepeatSample").UpdateValue(ref unitId);

            var coord = Texture.GetTextureRect().TopLeft;
            GetUniform<Vector2>(@"g_RepeatSampleCoord").UpdateValue(ref coord);

            var size = Texture.GetTextureRect().Size;

            GetUniform<Vector2>(@"g_RepeatSampleSize").UpdateValue(ref size);

            var textureSize = TextureSize;
            GetUniform<Vector2>("g_TextureSize").UpdateValue(ref textureSize);

            //var border = Border;
            //GetUniform<Vector2>("g_Border").UpdateValue(ref border);
        }
    }
}
