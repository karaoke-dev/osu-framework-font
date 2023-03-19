// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Runtime.InteropServices;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.Shaders.Types;
using osuTK;

namespace osu.Framework.Graphics.Shaders;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct RepeatMovingBackgroundParameters
{
    public UniformVector2 TexSize;
    public UniformVector2 RepeatSampleCoord;
    public UniformVector2 RepeatSampleSize;
    public UniformVector2 DisplaySize;
    public UniformVector2 DisplayBorder;
    public UniformVector2 Speed;
    public UniformFloat Time;
    public UniformFloat Mix;
}

public class RepeatMovingBackgroundShader : CustomizedShader<RepeatMovingBackgroundParameters>, IHasCurrentTime, IHasTextureSize
{
    public override string ShaderName => "RepeatMovingBackground";

    public Texture? Texture { get; set; }

    public Vector2 TextureSize { get; set; }

    public Vector2 TextureDisplaySize { get; set; } = new(10);

    public Vector2 TextureDisplayBorder { get; set; }

    public Vector2 Speed { get; set; }

    public float CurrentTime { get; set; }

    public float Mix { get; set; } = 1f;

    public override void ApplyValue()
    {
        if (Texture == null)
            return;

        // where this 1 is from? I have no clue.
        Texture.Bind(1);

        UniformBuffer.Data = new RepeatMovingBackgroundParameters
        {
            TexSize = TextureSize,
            RepeatSampleCoord = Texture.GetTextureRect().TopLeft,
            RepeatSampleSize = Texture.GetTextureRect().Size,
            DisplaySize = TextureDisplaySize,
            DisplayBorder = TextureDisplayBorder,
            Speed = Speed,
            Time = CurrentTime,
            Mix = Math.Clamp(Mix, 0, 1),
        };

        OriginShader.BindUniformBlock("m_RepeatMovingBackgroundParameters", UniformBuffer);
    }
}
