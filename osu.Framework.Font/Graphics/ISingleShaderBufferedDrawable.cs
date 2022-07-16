// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Graphics
{
    public interface ISingleShaderBufferedDrawable : IBufferedDrawable
    {
        IShader? Shader { get; }
    }
}
