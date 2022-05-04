// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Reflection;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.OpenGL;
using osu.Framework.Graphics.OpenGL.Buffers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics
{
    public abstract class CustomizedShaderBufferedDrawNode : BufferedDrawNode
    {
        private readonly double loadTime;

        protected CustomizedShaderBufferedDrawNode(IBufferedDrawable source, DrawNode child, BufferedDrawNodeSharedData sharedData)
            : base(source, child, sharedData)
        {
            loadTime = Source.Clock.CurrentTime;
        }

        protected static bool ContainTimePropertyShader(IShader shader)
        {
            switch (shader)
            {
                case IHasCurrentTime _:
                case IStepShader stepShader when stepShader.StepShaders.Any(s => s is IHasCurrentTime):
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

        protected void RenderShader(IShader shader, FrameBuffer current, FrameBuffer target)
        {
            GLWrapper.SetBlend(BlendingParameters.None);

            using (BindFrameBuffer(target))
            {
                if (shader is IHasTextureSize)
                {
                    var size = current.Size;
                    shader.GetUniform<Vector2>(@"g_TexSize").UpdateValue(ref size);
                }

                if (shader is IHasInflationPercentage)
                {
                    var localInflationAmount = DrawInfo.Matrix.ExtractScale().X;
                    shader.GetUniform<float>(@"g_InflationPercentage").UpdateValue(ref localInflationAmount);
                }

                if (shader is IHasCurrentTime)
                {
                    var currentTime = (float)(Source.Clock.CurrentTime - loadTime) / 1000;
                    shader.GetUniform<float>("g_Time").UpdateValue(ref currentTime);
                }

                if (shader is ICustomizedShader customizedShader)
                    customizedShader.ApplyValue();

                shader.Bind();
                DrawFrameBuffer(current, new RectangleF(0, 0, current.Texture.Width, current.Texture.Height), ColourInfo.SingleColour(Color4.White));
                shader.Unbind();
            }
        }
    }
}
