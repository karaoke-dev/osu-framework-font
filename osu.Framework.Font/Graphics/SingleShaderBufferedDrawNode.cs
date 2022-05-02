// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Reflection;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osuTK.Graphics;

namespace osu.Framework.Graphics
{
    public class SingleShaderBufferedDrawNode : BufferedDrawNode
    {
        protected new ISingleShaderBufferedDrawable Source => (ISingleShaderBufferedDrawable)base.Source;

        private readonly double loadTime;

        public SingleShaderBufferedDrawNode(ISingleShaderBufferedDrawable source, DrawNode child, BufferedDrawNodeSharedData sharedData)
            : base(source, child, sharedData)
        {
            loadTime = Source.Clock.CurrentTime;
        }

        protected override long GetDrawVersion()
        {
            // if contains shader that need to apply time, then need to force run populate contents in each frame.
            if (ContainTimePropertyShader(Source.Shader))
            {
                ResetDrawVersion();
            }

            return base.GetDrawVersion();
        }

        protected static bool ContainTimePropertyShader(IShader shader)
        {
            switch (shader)
            {
                case IApplicableToCurrentTime _:
                case IStepShader stepShader when stepShader.StepShaders.Any(s => s is IApplicableToCurrentTime):
                    return true;

                default:
                    return false;
            }
        }

        protected void ResetDrawVersion()
        {
            // todo : use better way to reset draw version.
            var prop = typeof(BufferedDrawNodeSharedData).GetField("DrawVersion", BindingFlags.Instance | BindingFlags.NonPublic);
            if (prop == null)
                throw new NullReferenceException();

            prop.SetValue(SharedData, -1);
        }

        protected override void PopulateContents()
        {
            base.PopulateContents();

            GLWrapper.PushScissorState(false);

            drawFrameBuffer();

            GLWrapper.PopScissorState();
        }

        protected override void DrawContents()
        {
            DrawFrameBuffer(SharedData.CurrentEffectBuffer, DrawRectangle, DrawColourInfo.Colour);
        }

        private void drawFrameBuffer()
        {
            var shader = Source.Shader;

            switch (shader)
            {
                case null:
                    return;

                case IStepShader stepShader:
                {
                    var stepShaders = stepShader.StepShaders;

                    foreach (var s in stepShaders)
                    {
                        renderShader(s);
                    }

                    break;
                }

                default:
                    renderShader(shader);
                    break;
            }

            void renderShader(IShader shader)
            {
                var current = SharedData.CurrentEffectBuffer;
                var target = SharedData.GetNextEffectBuffer();

                GLWrapper.SetBlend(BlendingParameters.None);

                using (BindFrameBuffer(target))
                {
                    if (shader is ICustomizedShader customizedShader)
                        customizedShader.ApplyValue(current);

                    if (shader is IApplicableToCurrentTime clockShader)
                    {
                        var time = (float)(Source.Clock.CurrentTime - loadTime) / 1000;
                        clockShader.ApplyCurrentTime(time);
                    }

                    shader.Bind();
                    DrawFrameBuffer(current, new RectangleF(0, 0, current.Texture.Width, current.Texture.Height), ColourInfo.SingleColour(Color4.White));
                    shader.Unbind();
                }
            }
        }
    }
}
