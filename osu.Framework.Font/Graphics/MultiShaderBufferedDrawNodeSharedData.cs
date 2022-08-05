﻿// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Textures;

namespace osu.Framework.Graphics
{
    public class MultiShaderBufferedDrawNodeSharedData : BufferedDrawNodeSharedData
    {
        private readonly Dictionary<IShader, IFrameBuffer> shaderBuffers = new Dictionary<IShader, IFrameBuffer>();

        public bool IsLatestFrameBuffer { get; set; }

        public IShader[] Shaders => shaderBuffers.Keys.ToArray();

        private readonly RenderBufferFormat[]? formats;

        public MultiShaderBufferedDrawNodeSharedData(RenderBufferFormat[]? formats = null, bool pixelSnapping = false)
            : base(0, formats, pixelSnapping)
        {
            this.formats = formats;
        }

        public void UpdateFrameBuffers(IRenderer renderer, IShader[] shaders)
        {
            if (IsLatestFrameBuffer)
                return;

            IsLatestFrameBuffer = true;

            // will not re-initialize if shader is not changed.
            if (shaderBuffers.Keys.SequenceEqual(shaders))
                return;

            // collect all frame buffer that needs to be disposed.
            var disposedFrameBuffer = shaderBuffers.Values.ToArray();
            shaderBuffers.Clear();

            foreach (var shader in shaders)
            {
                TextureFilteringMode filterMode = PixelSnapping ? TextureFilteringMode.Nearest : TextureFilteringMode.Linear;
                shaderBuffers.Add(shader, renderer.CreateFrameBuffer(formats, filterMode));
            }

            renderer.ScheduleDisposal(s =>
            {
                s.clearBuffers(disposedFrameBuffer);
            }, this);
        }

        public IFrameBuffer GetSourceFrameBuffer(IShader shader)
        {
            if (!(shader is IStepShader stepShader))
                return CurrentEffectBuffer;

            var fromShader = stepShader.FromShader;
            if (fromShader == null)
                return CurrentEffectBuffer;

            if (!shaderBuffers.ContainsKey(fromShader))
                throw new KeyNotFoundException();

            return shaderBuffers[fromShader];
        }

        public IFrameBuffer GetTargetFrameBuffer(IShader shader)
        {
            if (!shaderBuffers.ContainsKey(shader))
                throw new KeyNotFoundException();

            return shaderBuffers[shader];
        }

        public void UpdateBuffer(IShader shader, IFrameBuffer frameBuffer)
        {
            if (!shaderBuffers.ContainsKey(shader))
                throw new Exception();

            shaderBuffers[shader] = frameBuffer;
        }

        public IFrameBuffer[] GetDrawFrameBuffers()
            => shaderBuffers.Where(x =>
            {
                var (shader, frameBuffer) = x;

                // should not render disposed or not created frame buffer.
                if (frameBuffer.Texture == null)
                    return false;

                // should not draw the step shader if there's no content.
                if (shader is IStepShader stepShader)
                    return stepShader.StepShaders.Any() && stepShader.Draw;

                return true;
            }).Select(x => x.Value).ToArray();

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            clearBuffers(shaderBuffers.Values.ToArray());
        }

        private void clearBuffers(IFrameBuffer[] effectBuffers)
        {
            // dispose all frame buffer in array.
            foreach (var shaderBuffer in effectBuffers)
            {
                shaderBuffer.Dispose();
            }
        }
    }
}
