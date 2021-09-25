// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Buffers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osuTK.Graphics;

namespace osu.Framework.Graphics
{
    public class MultiShaderBufferedDrawNode : BufferedDrawNode
    {
        protected new IMultiShaderBufferedDrawable Source => (IMultiShaderBufferedDrawable)base.Source;

        private IReadOnlyList<IShader> shaders;

        public MultiShaderBufferedDrawNode(IBufferedDrawable source, DrawNode child, BufferedDrawNodeSharedData sharedData)
            : base(source, child, sharedData)
        {
        }

        public override void ApplyState()
        {
            base.ApplyState();
            shaders = Source.Shaders;
        }

        protected override void PopulateContents()
        {
            base.PopulateContents();
            drawFrameBuffer();
        }

        protected override void DrawContents()
        {
            DrawFrameBuffer(SharedData.CurrentEffectBuffer, DrawRectangle, Color4.White);
        }

        private void drawFrameBuffer()
        {
            if (!shaders.Any())
                return;

            FrameBuffer current = SharedData.CurrentEffectBuffer;
            FrameBuffer target = SharedData.GetNextEffectBuffer();

            GLWrapper.SetBlend(BlendingParameters.None);

            foreach (var shader in shaders)
            {
                var isFirst = shaders.ToList().IndexOf(shader) == 0;
                var source = isFirst ? current : target;

                using (BindFrameBuffer(target))
                {
                    UpdateUniforms(shader, source);

                    shader.Bind();
                    DrawFrameBuffer(source, new RectangleF(0, 0, source.Texture.Width, source.Texture.Height), ColourInfo.SingleColour(Color4.White));
                    shader.Unbind();
                }
            }
        }

        protected virtual void UpdateUniforms(IShader targetShader, FrameBuffer current)
        {
            if (targetShader is ICustomizedShader customizedShader)
                customizedShader.ApplyValue(current);
        }
    }
}
