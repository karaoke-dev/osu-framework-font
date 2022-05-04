// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Graphics.Shaders;
using osuTK.Graphics;

namespace osu.Framework.Graphics
{
    public class MultiShaderBufferedDrawNode : CustomizedShaderBufferedDrawNode
    {
        protected new IMultiShaderBufferedDrawable Source => (IMultiShaderBufferedDrawable)base.Source;

        protected new MultiShaderBufferedDrawNodeSharedData SharedData => (MultiShaderBufferedDrawNodeSharedData)base.SharedData;

        public MultiShaderBufferedDrawNode(IMultiShaderBufferedDrawable source, DrawNode child, MultiShaderBufferedDrawNodeSharedData sharedData)
            : base(source, child, sharedData)
        {
        }

        public override void ApplyState()
        {
            base.ApplyState();
            SharedData.UpdateFrameBuffers(Source.Shaders.ToArray());
        }

        protected override long GetDrawVersion()
        {
            // if contains shader that need to apply time, then need to force run populate contents in each frame.
            if (SharedData.Shaders.Any(ContainTimePropertyShader))
            {
                ResetDrawVersion();
            }

            return base.GetDrawVersion();
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
                base.DrawContents();
            }
        }

        private void drawFrameBuffer()
        {
            var shaders = SharedData.Shaders;
            if (!shaders.Any())
                return;

            foreach (var shader in shaders)
            {
                var current = SharedData.GetSourceFrameBuffer(shader);
                var target = SharedData.GetTargetFrameBuffer(shader);

                if (shader is IStepShader stepShader)
                {
                    var stepShaders = stepShader.StepShaders;

                    for (int i = 0; i < stepShaders.Count; i++)
                    {
                        // todo: it will cause the render issue if set the current and target shader into same shader.
                        RenderShader(stepShaders[i], i == 0 ? current : target, target);
                    }
                }
                else
                {
                    RenderShader(shader, current, target);
                }

                SharedData.UpdateBuffer(shader, target);
            }
        }
    }
}
