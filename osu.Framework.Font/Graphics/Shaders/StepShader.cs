// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Primitives;

namespace osu.Framework.Graphics.Shaders
{
    public class StepShader : IStepShader, IApplicableToCharacterSize, IApplicableToDrawRectangle
    {
        public string Name { get; set; } = null!;

        public IShader? FromShader { get; set; }

        private readonly List<ICustomizedShader> shaders = new List<ICustomizedShader>();

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

        public void Bind()
            => throw new NotSupportedException();

        public void Unbind()
            => throw new NotSupportedException();

        public Uniform<T> GetUniform<T>(string name) where T : struct, IEquatable<T>
            => throw new NotSupportedException();

        public bool IsLoaded
            => throw new NotSupportedException();

        public void ApplyValue()
            => throw new NotSupportedException();

        public RectangleF ComputeCharacterDrawRectangle(RectangleF originalCharacterDrawRectangle) =>
            StepShaders.OfType<IApplicableToCharacterSize>()
                       .Aggregate(originalCharacterDrawRectangle, (rectangle, shader) => shader.ComputeCharacterDrawRectangle(rectangle));

        public RectangleF ComputeDrawRectangle(RectangleF originDrawRectangle) =>
            StepShaders.OfType<IApplicableToDrawRectangle>()
                       .Aggregate(originDrawRectangle, (quad, shader) => shader.ComputeDrawRectangle(quad));
    }
}
