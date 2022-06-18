// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Framework.Graphics.Shaders
{
    /// <summary>
    /// Shader with customized property
    /// </summary>
    public abstract class CustomizedShader : ICustomizedShader
    {
        private IShader shader = null!;

        public void AttachOriginShader(IShader originShader)
        {
            shader = originShader ?? throw new ArgumentNullException(nameof(originShader));
        }

        public void Bind() => shader.Bind();

        public void Unbind() => shader.Unbind();

        public Uniform<T> GetUniform<T>(string name) where T : struct, IEquatable<T>
            => shader.GetUniform<T>(name);

        public bool IsLoaded => shader.IsLoaded;

        public abstract void ApplyValue();
    }
}
