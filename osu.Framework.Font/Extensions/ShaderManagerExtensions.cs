// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Extensions
{
    public static class ShaderManagerExtensions
    {
        public static T LocalInternalShader<T>(this ShaderManager shaderManager) where T : InternalShader, new()
        {
            var internalShader = new T();
            var shader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, internalShader.ShaderName);
            internalShader.AttachOriginShader(shader);
            return internalShader;
        }
    }
}
