// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
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
        public readonly IDictionary<IShader, FrameBuffer> ShaderBuffers = new Dictionary<IShader, FrameBuffer>();

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
                    return stepShader.IsValid() && stepShader.Draw;

                return true;
            }).Select(x => x.Value).ToArray();
    }
}
