// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.OpenGL.Buffers;
using osuTK;

namespace osu.Framework.Graphics.Shaders
{
    public class RainbowShader : InternalShader, IApplicableToCurrentTime
    {
        public override string ShaderName => "Rainbow";

        public Vector2 Uv { get; set; } = new Vector2(0, 1);

        public float Speed { get; set; } = 1;

        public float Saturation { get; set; } = 0.5f;

        public float Brightness { get; set; } = 1f;

        // todo: this property might able to be removed because it can be replaced by other property.
        public float Section { get; set; } = 1f;

        public float Mix { get; set; } = 0.5f;

        public RainbowShader(IShader originShader)
            : base(originShader)
        {
        }

        public override void ApplyValue(FrameBuffer current)
        {
            var uv = Uv;
            GetUniform<Vector2>(@"u_uv").UpdateValue(ref uv);

            var speed = Speed;
            GetUniform<float>(@"u_speed").UpdateValue(ref speed);

            var saturation = Saturation;
            GetUniform<float>(@"u_saturation").UpdateValue(ref saturation);

            var brightness = Brightness;
            GetUniform<float>(@"u_brightness").UpdateValue(ref brightness);

            var section = Section;
            GetUniform<float>(@"u_section").UpdateValue(ref section);

            var mix = Mix;
            GetUniform<float>(@"u_mix").UpdateValue(ref mix);
        }

        public void ApplyCurrentTime(float currentTime)
        {
            GetUniform<float>("u_time").UpdateValue(ref currentTime);
        }
    }
}
