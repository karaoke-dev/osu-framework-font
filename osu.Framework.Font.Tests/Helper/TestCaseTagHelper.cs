// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using osu.Framework.Graphics.Sprites;

namespace osu.Framework.Font.Tests.Helper
{
    public static class TestCaseTagHelper
    {
        /// <summary>
        /// Process test case ruby/romaji string format into <see cref="PositionText"/>
        /// </summary>
        /// <example>
        /// [0,3]:ruby
        /// [0,3]:romaji
        /// </example>
        /// <param name="str">Position text string format</param>
        /// <returns><see cref="PositionText"/>Position text object</returns>
        public static PositionText ParsePositionText(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new PositionText();

            var regex = new Regex("(?<start>[-0-9]+),(?<end>[-0-9]+)]:(?<ruby>.*$)");
            var result = regex.Match(str);
            if (!result.Success)
                throw new ArgumentException(null, nameof(str));

            var startIndex = int.Parse(result.Groups["start"].Value);
            var endIndex = int.Parse(result.Groups["end"].Value);
            var text = result.Groups["ruby"].Value;

            return new PositionText(text, startIndex, endIndex);
        }

        /// <summary>
        /// Process test case time tag string format into <see cref="Tuple"/>
        /// </summary>
        /// <example>
        /// [0,start]:1000
        /// </example>
        /// <param name="str">Time tag string format</param>
        /// <returns><see cref="Tuple"/>Time tag object</returns>
        public static Tuple<double, TextIndex> ParseTimeTag(string str)
        {
            if (string.IsNullOrEmpty(str))
                return new Tuple<double, TextIndex>(0, new TextIndex());

            var regex = new Regex("(?<index>[-0-9]+),(?<state>start|end)]:(?<time>[-0-9]+|s*|)");
            var result = regex.Match(str);
            if (!result.Success)
                throw new RegexMatchTimeoutException(nameof(str));

            int index = int.Parse(result.Groups["index"].Value);
            var state = result.Groups["state"].Value == "start" ? TextIndex.IndexState.Start : TextIndex.IndexState.End;
            double time = string.IsNullOrEmpty(result.Groups["time"].Value) ? 0 : double.Parse(result.Groups["time"].Value);

            return new Tuple<double, TextIndex>(time, new TextIndex(index, state));
        }

        public static PositionText[] ParsePositionTexts(IEnumerable<string> strings)
            => strings?.Select(ParsePositionText).ToArray();

        public static IReadOnlyDictionary<double, TextIndex> ParseTimeTags(IEnumerable<string> strings)
            => strings?.Select(ParseTimeTag).ToDictionary(k => k.Item1, k => k.Item2);
    }
}
