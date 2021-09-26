// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;

namespace osu.Framework.Graphics.Shaders
{
    public class StepShader : IStepShader
    {
        public string Name { get; set; }

        public IShader FromShader { get; set; }

        private readonly List<IShader> shaders = new List<IShader>();

        public IReadOnlyList<IShader> StepShaders
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
            => StepShaders.FirstOrDefault()?.Bind();

        public void Unbind()
            => StepShaders.FirstOrDefault()?.Unbind();

        public Uniform<T> GetUniform<T>(string name) where T : struct, IEquatable<T>
            => throw new NotSupportedException();

        public bool IsLoaded => StepShaders.Any();

        public IReadOnlyList<IShader> GetStepShaders()
        {
            // should skip first shader because it already being drawed.
            return StepShaders.Skip(1).ToArray();
        }

        public bool IsValid() => StepShaders.Any();
    }
}
