// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Rendering;

namespace osu.Framework.Graphics.Shaders;

public interface ICustomizedShader
{
    /// <summary>
    /// Fragment shader name to load (`sh_` prefix is implicitly added)
    /// </summary>
    string ShaderName { get; }

    void ApplyValue();

    /// <summary>
    /// Make sure shader uniforms are ready for initialization and upload.
    /// </summary>
    void PrepareUniforms(IRenderer renderer);

    /// <summary>
    /// Sets the original shader that is going to be uniform-tweaked.
    /// </summary>
    void AttachOriginShader(IShader originShader);

    /// <summary>
    /// Binds this shader to be used for rendering.
    /// </summary>
    void Bind();

    /// <summary>
    /// Unbinds this shader.
    /// </summary>
    void Unbind();

    /// <summary>
    /// Whether this shader is ready for use.
    /// </summary>
    bool IsLoaded { get; }

    /// <summary>
    /// Whether this shader is currently bound.
    /// </summary>
    bool IsBound { get; }
}
