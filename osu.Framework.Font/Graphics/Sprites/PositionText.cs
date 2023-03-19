// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Framework.Graphics.Sprites;

public readonly struct PositionText : IEquatable<PositionText>
{
    public PositionText(string text, int startIndex, int endIndex)
    {
        Text = text;
        StartIndex = startIndex;
        EndIndex = endIndex;
    }

    public string Text { get; }

    public int StartIndex { get; }

    public int EndIndex { get; }

    public bool Equals(PositionText other)
        => StartIndex == other.StartIndex && EndIndex == other.EndIndex && Text == other.Text;

    public override bool Equals(object? obj)
    {
        if (obj is PositionText positionText)
            return Equals(positionText);

        // If compare object is not int or position text, then it's no need to be compared.
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Text, StartIndex, EndIndex);
    }

    public static bool operator ==(PositionText first, PositionText second) => first.Equals(second);

    public static bool operator !=(PositionText first, PositionText second) => !first.Equals(second);
}
