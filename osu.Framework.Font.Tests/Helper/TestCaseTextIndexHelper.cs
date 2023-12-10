// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Text.RegularExpressions;
using osu.Framework.Graphics.Sprites;

namespace osu.Framework.Font.Tests.Helper;

public class TestCaseTextIndexHelper
{
    public static TextIndex ParseTextIndex(string str)
    {
        var regex = new Regex("(?<index>[-0-9]+),(?<state>start|end)");
        var result = regex.Match(str);
        if (!result.Success)
            throw new RegexMatchTimeoutException(nameof(str));

        int index = int.Parse(result.Groups["index"].Value);
        var state = result.Groups["state"].Value == "start" ? TextIndex.IndexState.Start : TextIndex.IndexState.End;

        return new TextIndex(index, state);
    }
}
