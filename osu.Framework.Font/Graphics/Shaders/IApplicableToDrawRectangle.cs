// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Primitives;

namespace osu.Framework.Graphics.Shaders;

public interface IApplicableToDrawRectangle
{
    RectangleF ComputeDrawRectangle(RectangleF originDrawRectangle);
}
