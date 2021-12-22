// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.OpenGL.Buffers;

namespace osu.Framework.Graphics.Shaders
{
    public interface ICustomizedShader : IShader
    {
        void ApplyValue(FrameBuffer current);
    }
}
