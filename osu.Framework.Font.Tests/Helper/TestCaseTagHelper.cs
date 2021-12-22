// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
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

            return new PositionText
            {
                StartIndex = startIndex,
                EndIndex = endIndex,
                Text = text
            };
        }

        public static PositionText[] ParseParsePositionTexts(string[] strings)
            => strings?.Select(ParsePositionText).ToArray();
    }
}
