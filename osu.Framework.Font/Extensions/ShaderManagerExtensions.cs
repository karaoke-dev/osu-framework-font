// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Extensions;

public static class ShaderManagerExtensions
{
    public static T LocalCustomizedShader<T>(this ShaderManager shaderManager) where T : ICustomizedShader, new()
    {
        var customizedShader = new T();
        AttachShader(shaderManager, customizedShader);
        return customizedShader;
    }

    public static void AttachShader(this ShaderManager shaderManager, ICustomizedShader customizedShader)
    {
        var shader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, customizedShader.ShaderName);
        customizedShader.AttachOriginShader(shader);
    }
}
