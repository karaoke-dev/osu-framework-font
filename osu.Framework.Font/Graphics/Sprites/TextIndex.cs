// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Framework.Graphics.Sprites
{
    public struct TextIndex : IComparable<TextIndex>, IEquatable<TextIndex>
    {
        public int Index { get; set; }

        public IndexState State { get; set; }

        public TextIndex(int index = 0, IndexState state = IndexState.Start)
        {
            Index = index;
            State = state;
        }

        public int CompareTo(TextIndex other)
        {
            if (Index > other.Index)
                return 1;

            if (Index < other.Index)
                return -1;

            if (State == other.State)
                return 0;

            if (State == IndexState.Start)
                return -1;

            return 1;
        }

        public bool Equals(TextIndex other)
        {
            return Index == other.Index && State == other.State;
        }

        public override bool Equals(object obj)
        {
            if (obj is TextIndex tone)
                return Equals(tone);

            // If compare object is not int or tone, then it's no need to be compared.
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(TextIndex index1, TextIndex index2) => index1.Equals(index2);

        public static bool operator !=(TextIndex index1, TextIndex index2) => !index1.Equals(index2);

        public static bool operator >(TextIndex index1, TextIndex index2) => index1.CompareTo(index2) > 0;

        public static bool operator >=(TextIndex index1, TextIndex index2) => index1.CompareTo(index2) >= 0;

        public static bool operator <(TextIndex index1, TextIndex index2) => index1.CompareTo(index2) < 0;

        public static bool operator <=(TextIndex index1, TextIndex index2) => index1.CompareTo(index2) <= 0;

        public override string ToString()
        {
            return $"Index:{Index}, Start:{State}";
        }

        public enum IndexState
        {
            Start = 0,

            End = 1
        }
    }
}
