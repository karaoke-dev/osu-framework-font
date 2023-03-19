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
public record struct OutlineParameters
{
    public UniformVector4 Colour;
    public UniformVector4 OutlineColour;
    public UniformVector2 TexSize;
    public UniformFloat Radius;
    public UniformFloat InflationPercentage;
}

public class OutlineShader : CustomizedShader<OutlineParameters>, IApplicableToCharacterSize, IApplicableToDrawRectangle, IHasTextureSize, IHasInflationPercentage
{
    public override string ShaderName => "Outline";

    public Vector2 TextureSize { get; set; }

    public Color4 Colour { get; set; }

    public float Radius { get; set; }

    public Color4 OutlineColour { get; set; }

    public float InflationPercentage { get; set; }

    public override void ApplyValue()
    {
        UniformBuffer.Data = new OutlineParameters
        {
            TexSize = TextureSize,
            Colour = new Vector4(Colour.R, Colour.G, Colour.B, Colour.A),
            Radius = Radius,
            OutlineColour = new Vector4(OutlineColour.R, OutlineColour.G, OutlineColour.B, OutlineColour.A),
            InflationPercentage = InflationPercentage
        };

        OriginShader.BindUniformBlock("m_OutlineParameters", UniformBuffer);
    }

    public RectangleF ComputeCharacterDrawRectangle(RectangleF originalCharacterDrawRectangle)
        => originalCharacterDrawRectangle.Inflate(Math.Max(Radius, 0));

    public RectangleF ComputeDrawRectangle(RectangleF originDrawRectangle)
        => ComputeCharacterDrawRectangle(originDrawRectangle);
}
