// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Framework.Graphics.Sprites
{
    public struct TimeTagIndex : IComparable<TimeTagIndex>, IEquatable<TimeTagIndex>
    {
        public int Index { get; set; }

        public IndexState State { get; set; }

        public TimeTagIndex(int index = 0, IndexState state = IndexState.Start)
        {
            Index = index;
            State = state;
        }

        public int CompareTo(TimeTagIndex other)
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

        public bool Equals(TimeTagIndex other)
        {
            return Index == other.Index && State == other.State;
        }

        public override bool Equals(object obj)
        {
            if (obj is TimeTagIndex tone)
                return Equals(tone);

            // If compare object is not int or tone, then it's no need to be compared.
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(TimeTagIndex index1, TimeTagIndex index2) => index1.Equals(index2);

        public static bool operator !=(TimeTagIndex index1, TimeTagIndex index2) => !index1.Equals(index2);

        public static bool operator >(TimeTagIndex index1, TimeTagIndex index2) => index1.CompareTo(index2) > 0;

        public static bool operator >=(TimeTagIndex index1, TimeTagIndex index2) => index1.CompareTo(index2) >= 0;

        public static bool operator <(TimeTagIndex index1, TimeTagIndex index2) => index1.CompareTo(index2) < 0;

        public static bool operator <=(TimeTagIndex index1, TimeTagIndex index2) => index1.CompareTo(index2) <= 0;

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
