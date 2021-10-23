// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Extensions
{
    public static class ShaderManagerExtensions
    {
        public static T LocalInternalShader<T>(this ShaderManager shaderManager) where T : InternalShader
        {
            var shaderName = ((T)Activator.CreateInstance(typeof(T), default(IShader))).ShaderName;
            var shader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, shaderName);
            return (T)Activator.CreateInstance(typeof(T), shader);
        }
    }
}
