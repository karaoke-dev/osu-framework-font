// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Text.RegularExpressions;
using osuTK;

namespace osu.Framework.Font.Tests.Helper;

public static class TestCaseVectorHelper
{
    /// <summary>
    /// Process test case vector2 string format into <see cref="Vector2"/>
    /// </summary>
    /// <example>
    /// (0,0)
    /// (10m 10)
    /// </example>
    /// <param name="str">Vector2 string format</param>
    /// <returns><see cref="Vector2"/>Vector2 object</returns>
    public static Vector2 ParseVector2(string str)
    {
        if (string.IsNullOrEmpty(str))
            return new Vector2();

        var regex = new Regex("(?<x>[-0-9]+),(?<y>[-0-9]+)");
        var result = regex.Match(str);
        if (!result.Success)
            throw new ArgumentException(null, nameof(str));

        var x = float.Parse(result.Groups["x"].Value);
        var y = float.Parse(result.Groups["y"].Value);

        return new Vector2(x, y);
    }
}
