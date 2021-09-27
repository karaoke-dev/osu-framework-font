﻿// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.OpenGL.Buffers;
using osu.Framework.Graphics.Shaders;
using osuTK.Graphics.ES30;

namespace osu.Framework.Graphics
{
    public class MultiShaderBufferedDrawNodeSharedData : BufferedDrawNodeSharedData
    {
        private readonly Dictionary<IShader, FrameBuffer> shaderBuffers = new Dictionary<IShader, FrameBuffer>();

        public IReadOnlyDictionary<IShader, FrameBuffer> ShaderBuffers => shaderBuffers;

        private readonly RenderbufferInternalFormat[] formats;

        public MultiShaderBufferedDrawNodeSharedData(RenderbufferInternalFormat[] formats = null, bool pixelSnapping = false)
            : base(0, formats, pixelSnapping)
        {
            this.formats = formats;
        }

        public FrameBuffer CreateFrameBuffer()
        {
            var filterMode = PixelSnapping ? All.Nearest : All.Linear;
            return new FrameBuffer(formats, filterMode);
        }

        public FrameBuffer[] GetDrawFrameBuffers()
            => ShaderBuffers.Where(x =>
            {
                var shader = x.Key;
                if (shader is IStepShader stepShader)
                    return stepShader.StepShaders.Any() && stepShader.Draw;

                return true;
            }).Select(x => x.Value).ToArray();

        public void AddOrReplaceBuffer(IShader shader, FrameBuffer frameBuffer)
        {
            if (shaderBuffers.ContainsKey(shader))
            {
                shaderBuffers[shader] = frameBuffer;
            }
            else
            {
                shaderBuffers.Add(shader, frameBuffer);
            }
        }

        public void ClearBuffer()
        {
            shaderBuffers.Clear();
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            // clear all frame in the dictionary.
            foreach (var shaderBuffer in shaderBuffers)
            {
                shaderBuffer.Value.Dispose();
            }

            shaderBuffers.Clear();
        }
    }
}
