// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.IO;
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

        protected new MultiShaderBufferedDrawNodeSharedData SharedData => (MultiShaderBufferedDrawNodeSharedData)base.SharedData;

        private IReadOnlyList<IShader> shaders;

        public MultiShaderBufferedDrawNode(IBufferedDrawable source, DrawNode child, MultiShaderBufferedDrawNodeSharedData sharedData)
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
            var drawFrameBuffers = SharedData.GetDrawFrameBuffers().Reverse().ToArray();

            if (drawFrameBuffers.Any())
            {
                foreach (var frameBuffer in drawFrameBuffers)
                {
                    DrawFrameBuffer(frameBuffer, DrawRectangle, Color4.White);
                }
            }
            else
            {
                // should draw origin content if no shader effects.
                DrawFrameBuffer(SharedData.CurrentEffectBuffer, DrawRectangle, Color4.White);
            }
        }

        private void drawFrameBuffer()
        {
            if (!shaders.Any())
                return;

            GLWrapper.SetBlend(BlendingParameters.None);

            SharedData.ShaderBuffers.Clear();

            foreach (var shader in shaders)
            {
                var current = getSourceFrameBuffer(shader);
                var target = SharedData.CreateFrameBuffer();

                if (shader is IStepShader stepShader)
                {
                    var stepShaders = stepShader.StepShaders;

                    for (int i = 0; i < stepShaders.Count; i++)
                    {
                        renderShader(stepShaders[i], i == 0 ? current : target, target);
                    }
                }
                else
                {
                    renderShader(shader, current, target);
                }

                // todo: not really sure will cause memory issue.
                SharedData.ShaderBuffers.Add(shader, target);
            }

            void renderShader(IShader shader, FrameBuffer current, FrameBuffer target)
            {
                using (BindFrameBuffer(target))
                {
                    if (shader is ICustomizedShader customizedShader)
                        customizedShader.ApplyValue(current);

                    shader.Bind();
                    DrawFrameBuffer(current, new RectangleF(0, 0, current.Texture.Width, current.Texture.Height), ColourInfo.SingleColour(Color4.White));
                    shader.Unbind();
                }
            }

            FrameBuffer getSourceFrameBuffer(IShader targetShader)
            {
                if (!(targetShader is IStepShader stepShader))
                    return SharedData.CurrentEffectBuffer;

                var fromShader = stepShader.FromShader;
                if (fromShader == null)
                    return SharedData.CurrentEffectBuffer;

                var shaderBuffers = SharedData.ShaderBuffers;
                if (!shaderBuffers.ContainsKey(fromShader))
                    throw new DirectoryNotFoundException("Frame buffer does not found in target shader.");

                return shaderBuffers[fromShader];
            }
        }
    }
}
