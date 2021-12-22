// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics.OpenGL.Buffers;

namespace osu.Framework.Graphics.Shaders
{
    public interface IStepShader : ICustomizedShader
    {
        /// <summary>
        /// Render <see cref="FrameBuffer"/> from target <see cref="IShader"/> result.
        /// </summary>
        IShader FromShader { get; }

        /// <summary>
        /// List if <see cref="ICustomizedShader"/>, will pass <see cref="FrameBuffer"/> to each shaders.
        /// </summary>
        IReadOnlyList<ICustomizedShader> StepShaders { get; }

        /// <summary>
        /// Should draw <see cref="FrameBuffer"/> or not.
        /// </summary>
        bool Draw { get; }
    }
}
