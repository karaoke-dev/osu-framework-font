// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Framework.Graphics.Sprites
{
    public interface IHasTexture
    {
        ILyricTexture TextTexture { get; set; }

        ILyricTexture ShadowTexture { get; set; }

        ILyricTexture BorderTexture { get; set; }
    }
}
