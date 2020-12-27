// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

namespace osu.Framework.Graphics.Sprites
{
    public struct PositionText
    {
        public PositionText(string text, int startIndex, int endIndex)
        {
            Text = text;
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public string Text { get; set; }

        public int StartIndex { get; set; }

        public int EndIndex { get; set; }
    }
}
