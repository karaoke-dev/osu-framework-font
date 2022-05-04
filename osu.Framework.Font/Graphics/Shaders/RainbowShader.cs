// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

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

        public override void ApplyValue()
        {
            var uv = Uv;
            GetUniform<Vector2>(@"g_Uv").UpdateValue(ref uv);

            var speed = Speed;
            GetUniform<float>(@"g_Speed").UpdateValue(ref speed);

            var saturation = Saturation;
            GetUniform<float>(@"g_Saturation").UpdateValue(ref saturation);

            var brightness = Brightness;
            GetUniform<float>(@"g_Brightness").UpdateValue(ref brightness);

            var section = Section;
            GetUniform<float>(@"g_Section").UpdateValue(ref section);

            var mix = Mix;
            GetUniform<float>(@"g_Mix").UpdateValue(ref mix);
        }

        public void ApplyCurrentTime(float currentTime)
        {
            GetUniform<float>("g_Time").UpdateValue(ref currentTime);
        }
    }
}
