// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Primitives;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Shaders;

public class OutlineShader : InternalShader, IApplicableToCharacterSize, IApplicableToDrawRectangle, IHasTextureSize, IHasInflationPercentage
{
    public override string ShaderName => "Outline";

    public Color4 Colour { get; set; }

    public float Radius { get; set; }

    public Color4 OutlineColour { get; set; }

    public override void ApplyValue()
    {
        var colourMatrix = new Vector4(Colour.R, Colour.G, Colour.B, Colour.A);
        GetUniform<Vector4>(@"g_Colour").UpdateValue(ref colourMatrix);

        float radius = Radius;
        GetUniform<float>(@"g_Radius").UpdateValue(ref radius);

        var outlineColourMatrix = new Vector4(OutlineColour.R, OutlineColour.G, OutlineColour.B, OutlineColour.A);
        GetUniform<Vector4>(@"g_OutlineColour").UpdateValue(ref outlineColourMatrix);
    }

    public RectangleF ComputeCharacterDrawRectangle(RectangleF originalCharacterDrawRectangle)
        => originalCharacterDrawRectangle.Inflate(Math.Max(Radius, 0));

    public RectangleF ComputeDrawRectangle(RectangleF originDrawRectangle)
        => ComputeCharacterDrawRectangle(originDrawRectangle);
}
