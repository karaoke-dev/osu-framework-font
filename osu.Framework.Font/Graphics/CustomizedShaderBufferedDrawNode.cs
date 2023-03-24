// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.Shaders.Types;
using osuTK.Graphics;

namespace osu.Framework.Graphics;

public abstract class CustomizedShaderBufferedDrawNode : BufferedDrawNode
{
    private readonly double loadTime;

    protected CustomizedShaderBufferedDrawNode(IBufferedDrawable source, DrawNode child, BufferedDrawNodeSharedData sharedData)
        : base(source, child, sharedData)
    {
        loadTime = Source.Clock.CurrentTime;
    }

    protected static bool ContainTimePropertyShader(ICustomizedShader shader)
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
            throw new NotSupportedException("Underlying data structure has changed, KaraokeFont needs an update");

        prop.SetValue(SharedData, -1);
    }

    private IUniformBuffer<SharedParameters>? sharedParametersBuffer;

    protected void RenderShader(IRenderer renderer, ICustomizedShader shader, IFrameBuffer current, IFrameBuffer target)
    {
        sharedParametersBuffer ??= renderer.CreateUniformBuffer<SharedParameters>();

        renderer.SetBlend(BlendingParameters.None);

        using (BindFrameBuffer(target))
        {
            if (shader is IHasTextureSize)
            {
                var size = current.Size;
                sharedParametersBuffer.Data = sharedParametersBuffer.Data with
                {
                    TexSize = size
                };
            }

            if (shader is IHasInflationPercentage)
            {
                var localInflationAmount = DrawInfo.Matrix.ExtractScale().X;
                sharedParametersBuffer.Data = sharedParametersBuffer.Data with
                {
                    InflationPercentage = localInflationAmount
                };
            }

            if (shader is IHasCurrentTime)
            {
                var currentTime = (float)(Source.Clock.CurrentTime - loadTime) / 1000;
                sharedParametersBuffer.Data = sharedParametersBuffer.Data with
                {
                    Time = currentTime
                };
            }

            if (shader is ICustomizedShader customizedShader)
                customizedShader.ApplyValue(renderer);

            shader.BindUniformBlock("m_SharedParameters", sharedParametersBuffer);
            shader.Bind();

            renderer.DrawFrameBuffer(current, new RectangleF(0, 0, current.Texture.Width, current.Texture.Height), ColourInfo.SingleColour(Color4.White));

            shader.Unbind();
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private record struct SharedParameters
    {
        public UniformVector2 TexSize;
        public UniformFloat InflationPercentage;
        public UniformFloat Time;
    }
}
