// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Runtime.InteropServices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders.Types;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Shaders;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public record struct ShadowParameters
{
    public UniformVector4 Colour;
    public UniformVector2 TexSize;
    public UniformVector2 Offset;
    public UniformFloat InflationPercentage;
    private readonly UniformPadding12 pad1;
}

public class ShadowShader : CustomizedShader<ShadowParameters>, IApplicableToDrawRectangle, IHasTextureSize, IHasInflationPercentage
{
    public override string ShaderName => "Shadow";

    public Vector2 TextureSize { get; set; }

    public Color4 ShadowColour { get; set; }

    public Vector2 ShadowOffset { get; set; }

    public float InflationPercentage { get; set; }

    public override void ApplyValue()
    {
        UniformBuffer.Data = new ShadowParameters
        {
            TexSize = TextureSize,
            Colour = new Vector4(ShadowColour.R, ShadowColour.G, ShadowColour.B, ShadowColour.A),
            Offset = ShadowOffset,
            InflationPercentage = InflationPercentage
        };

        OriginShader.BindUniformBlock("m_ShadowParameters", UniformBuffer);
    }

    public RectangleF ComputeDrawRectangle(RectangleF originDrawRectangle)
        => originDrawRectangle.Inflate(new MarginPadding
        {
            Left = Math.Max(-ShadowOffset.X, 0),
            Right = Math.Max(ShadowOffset.X, 0),
            Top = Math.Max(-ShadowOffset.Y, 0),
            Bottom = Math.Max(ShadowOffset.Y, 0),
        });
}
