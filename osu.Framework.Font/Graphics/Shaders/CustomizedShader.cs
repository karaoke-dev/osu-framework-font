// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

using osu.Framework.Graphics.Rendering;

namespace osu.Framework.Graphics.Shaders;

/// <summary>
/// Shader with customized property
/// </summary>
public abstract class CustomizedShader<TUniform> : ICustomizedShader
    where TUniform : unmanaged, IEquatable<TUniform>
{
    private IShader shader = null!;

    public abstract string ShaderName { get; }

    private IUniformBuffer<TUniform>? uniformBuffer;

    protected IUniformBuffer<TUniform> UniformBuffer => uniformBuffer!;

    protected IShader OriginShader => shader;

    public void PrepareUniforms(IRenderer renderer)
    {
        uniformBuffer ??= renderer.CreateUniformBuffer<TUniform>();
    }

    public void AttachOriginShader(IShader originShader)
    {
        shader = originShader ?? throw new ArgumentNullException(nameof(originShader));
    }

    public void Bind() => shader.Bind();

    public void Unbind() => shader.Unbind();

    public bool IsLoaded => shader.IsLoaded;

    public bool IsBound { get; private set; }

    public abstract void ApplyValue();

    public void Dispose()
    {
    }
}
