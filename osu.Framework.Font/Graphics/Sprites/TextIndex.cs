// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Framework.Graphics.Sprites;

public readonly struct TextIndex : IComparable<TextIndex>, IEquatable<TextIndex>
{
    public int Index { get; }

    public IndexState State { get; }

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
        if (obj is TextIndex textIndex)
            return Equals(textIndex);

        // If compare object is not int or text index, then it's no need to be compared.
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Index, State);
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
