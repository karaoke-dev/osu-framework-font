﻿// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Rendering;

namespace osu.Framework.Graphics.Shaders;

public class StepShader : IStepShader, IApplicableToCharacterSize, IApplicableToDrawRectangle
{
    public string Name { get; set; } = null!;

    // TODO: unused, remove this variable (IStepShader should probably not inherit from ICustomizedShader)
    public string ShaderName => throw new NotSupportedException();

    public ICustomizedShader? FromShader { get; set; }

    private readonly List<ICustomizedShader> shaders = new();

    public IReadOnlyList<ICustomizedShader> StepShaders
    {
        get => shaders;
        set
        {
            if (value.OfType<IStepShader>().Any())
                throw new InvalidCastException($"Cannot have any {nameof(IStepShader)} as step.");

            shaders.Clear();
            shaders.AddRange(value);
        }
    }

    public bool Draw { get; set; } = true;

    public void PrepareUniforms(IRenderer renderer)
        => throw new NotSupportedException();

    public void Bind()
        => throw new NotSupportedException();

    public void Unbind()
        => throw new NotSupportedException();

    public bool IsLoaded
        => throw new NotSupportedException();

    public bool IsBound { get; private set; }

    public void AttachOriginShader(IShader renderer)
        => throw new NotSupportedException();

    public void ApplyValue()
        => throw new NotSupportedException();

    public RectangleF ComputeCharacterDrawRectangle(RectangleF originalCharacterDrawRectangle) =>
        StepShaders.OfType<IApplicableToCharacterSize>()
                   .Aggregate(originalCharacterDrawRectangle, (rectangle, shader) => shader.ComputeCharacterDrawRectangle(rectangle));

    public RectangleF ComputeDrawRectangle(RectangleF originDrawRectangle) =>
        StepShaders.OfType<IApplicableToDrawRectangle>()
                   .Aggregate(originDrawRectangle, (quad, shader) => shader.ComputeDrawRectangle(quad));

    public void Dispose()
    {
    }
}
