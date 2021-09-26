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
            => throw new NotSupportedException();

        public void Unbind()
            => throw new NotSupportedException();

        public Uniform<T> GetUniform<T>(string name) where T : struct, IEquatable<T>
            => throw new NotSupportedException();

        public bool IsLoaded
            => throw new NotSupportedException();
    }
}
