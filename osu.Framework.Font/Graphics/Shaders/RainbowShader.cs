// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osuTK;

using System;
using System.Runtime.InteropServices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders.Types;

namespace osu.Framework.Graphics.Shaders;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct RainbowParameters
{
    public UniformVector2 Uv;
    public UniformFloat Speed;
    public UniformFloat Time;
    public UniformFloat Saturation;
    public UniformFloat Brightness;
    public UniformFloat Section;
    public UniformFloat Mix;
}

public class RainbowShader : CustomizedShader<RainbowParameters>, IHasCurrentTime
{
    public override string ShaderName => "Rainbow";

    public Vector2 Uv { get; set; } = new(0, 1);

    public float Speed { get; set; } = 1;

    public float CurrentTime { get; set; }

    public float Saturation { get; set; } = 0.5f;

    public float Brightness { get; set; } = 1f;

    // todo: this property might able to be removed because it can be replaced by other property.
    public float Section { get; set; } = 1f;

    public float Mix { get; set; } = 0.5f;

    public override void ApplyValue()
    {
        UniformBuffer.Data = new RainbowParameters
        {
            Uv = Uv,
            Speed = Speed,
            Time = CurrentTime,
            Saturation = Saturation,
            Brightness = Brightness,
            Section = Section,
            Mix = Mix
        };

        OriginShader.BindUniformBlock("m_RainbowParameters", UniformBuffer);
    }
}
