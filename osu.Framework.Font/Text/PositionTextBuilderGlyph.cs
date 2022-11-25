// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Runtime.CompilerServices;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace osu.Framework.Text;

/// <summary>
/// A <see cref="ITexturedCharacterGlyph"/> provided as final <see cref="PositionText"/> position conversion of <see cref="TextBuilderGlyph"/>.
/// </summary>
public readonly struct PositionTextBuilderGlyph : ITexturedCharacterGlyph
{
    public Texture Texture => glyph.Texture;
    public float XOffset => glyph.XOffset;

    public float YOffset => glyph.YOffset;
    public float XAdvance => glyph.XAdvance;

    public float Baseline => glyph.Baseline;
    public float Width => glyph.Width;

    public float Height => glyph.Height;
    public char Character => glyph.Character;

    private readonly TextBuilderGlyph glyph;

    /// <summary>
    /// The rectangle for the character to be drawn in.
    /// </summary>
    public RectangleF DrawRectangle { get; }

    internal PositionTextBuilderGlyph(TextBuilderGlyph glyph, Vector2 position)
    {
        this = default;

        this.glyph = glyph;

        DrawRectangle = new RectangleF(position, glyph.DrawRectangle.Size);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float GetKerning<T>(T lastGlyph) where T : ICharacterGlyph
        => glyph.GetKerning(lastGlyph);
}
