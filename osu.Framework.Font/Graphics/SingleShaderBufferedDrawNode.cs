// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;

namespace osu.Framework.Graphics
{
    public class SingleShaderBufferedDrawNode : CustomizedShaderBufferedDrawNode
    {
        protected new ISingleShaderBufferedDrawable Source => (ISingleShaderBufferedDrawable)base.Source;

        public SingleShaderBufferedDrawNode(ISingleShaderBufferedDrawable source, DrawNode child, BufferedDrawNodeSharedData sharedData)
            : base(source, child, sharedData)
        {
        }

        protected override long GetDrawVersion()
        {
            // if contains shader that need to apply time, then need to force run populate contents in each frame.
            if (Source.Shader != null && ContainTimePropertyShader(Source.Shader))
            {
                ResetDrawVersion();
            }

            return base.GetDrawVersion();
        }

        protected override void PopulateContents(IRenderer renderer)
        {
            base.PopulateContents(renderer);

            if (Source.Shader != null)
                drawFrameBuffer(renderer, Source.Shader);
        }

        protected override void DrawContents(IRenderer renderer)
        {
            renderer.DrawFrameBuffer(SharedData.CurrentEffectBuffer, DrawRectangle, DrawColourInfo.Colour);
        }

        private void drawFrameBuffer(IRenderer renderer, IShader shader)
        {
            switch (shader)
            {
                case null:
                    return;

                case IStepShader stepShader:
                {
                    var stepShaders = stepShader.StepShaders;

                    foreach (var s in stepShaders)
                    {
                        drawFrameBuffer(renderer, s);
                    }

                    break;
                }

                default:
                    var current = SharedData.CurrentEffectBuffer;
                    var target = SharedData.GetNextEffectBuffer();
                    RenderShader(renderer, shader, current, target);
                    break;
            }
        }
    }
}
