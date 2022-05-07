// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics.Sprites;

namespace osu.Framework.Utils
{
    public static class TextIndexUtils
    {
        public static TextIndex Clamp(TextIndex value, int minValue, int maxValue)
        {
            return new TextIndex(Math.Clamp(value.Index, minValue, maxValue), value.State);
        }

        public static int ToStringIndex(TextIndex index)
        {
            if (index.State == TextIndex.IndexState.Start)
                return index.Index;

            return index.Index + 1;
        }

        public static TextIndex.IndexState ReverseState(TextIndex.IndexState state)
        {
            return state == TextIndex.IndexState.Start ? TextIndex.IndexState.End : TextIndex.IndexState.Start;
        }

        public static TextIndex GetPreviousIndex(TextIndex originIndex)
        {
            int previousIndex = ToStringIndex(originIndex) - 1;
            var previousState = ReverseState(originIndex.State);
            return new TextIndex(previousIndex, previousState);
        }

        public static TextIndex GetNextIndex(TextIndex originIndex)
        {
            int nextIndex = ToStringIndex(originIndex);
            var nextState = ReverseState(originIndex.State);
            return new TextIndex(nextIndex, nextState);
        }
    }
}
