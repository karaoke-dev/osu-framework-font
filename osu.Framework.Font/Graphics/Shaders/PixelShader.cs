// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Rendering;
using osuTK;

namespace osu.Framework.Graphics.Shaders;

public class PixelShader : InternalShader, IHasTextureSize, IHasInflationPercentage
{
    public override string ShaderName => "Pixel";

    public Vector2 Size { get; set; } = new(5);

    public override void ApplyValue(IRenderer renderer)
    {
        var size = Size;
        GetUniform<Vector2>(@"g_Size").UpdateValue(ref size);
    }
}
