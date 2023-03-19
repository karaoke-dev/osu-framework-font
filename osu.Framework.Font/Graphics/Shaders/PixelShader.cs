// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.InteropServices;
using osu.Framework.Graphics.Shaders.Types;
using osuTK;

namespace osu.Framework.Graphics.Shaders;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct PixelShaderParameters
{
    public UniformVector2 TexSize;
    public UniformVector2 Size;
    public UniformFloat InflationPercentage;
}

public class PixelShader : CustomizedShader<PixelShaderParameters>, IHasTextureSize, IHasInflationPercentage
{
    public override string ShaderName => "Pixel";

    public Vector2 TextureSize { get; set; }

    public Vector2 Size { get; set; } = new(5);

    public float InflationPercentage { get; set; }

    public override void ApplyValue()
    {
        UniformBuffer.Data = new PixelShaderParameters
        {
            TexSize = TextureSize,
            Size = Size,
            InflationPercentage = InflationPercentage
        };

        OriginShader.BindUniformBlock("m_PixelParameters", UniformBuffer);
    }
}
