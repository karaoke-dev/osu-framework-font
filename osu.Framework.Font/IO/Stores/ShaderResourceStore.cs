// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Reflection;
using osu.Framework.Graphics.Sprites;

namespace osu.Framework.IO.Stores
{
    public class ShaderResourceStore : DllResourceStore
    {
        public ShaderResourceStore()
            : base(typeof(LyricSpriteText).Assembly)
        {
            var property = typeof(DllResourceStore).GetField("prefix", BindingFlags.NonPublic | BindingFlags.Instance);
            if (property == null)
                throw new Exception();

            property.SetValue(this, "osu.Framework");
        }
    }
}
