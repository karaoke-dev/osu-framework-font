// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Text;
using osuTK;

namespace osu.Framework.Font.Tests.Text
{
    [TestFixture]
    public class PositionTextBuilderTest
    {
        private const float font_size = 1;

        private const float x_offset = 1;
        private const float y_offset = 2;
        private const float x_advance = 3;
        private const float width = 4;
        private const float baseline = 5;
        private const float height = 6;
        private const float kerning = -7;

        private const float b_x_offset = 8;
        private const float b_y_offset = 9;
        private const float b_x_advance = 10;
        private const float b_width = 11;
        private const float b_baseline = 12;
        private const float b_height = 13;
        private const float b_kerning = -14;

        private const float m_x_offset = 15;
        private const float m_y_offset = 16;
        private const float m_x_advance = 17;
        private const float m_width = 18;
        private const float m_baseline = 19;
        private const float m_height = 20;
        private const float m_kerning = -21;

        private static readonly Vector2 spacing = new(19, 20);

        private static readonly TestFontUsage normal_font = new("test");
        private static readonly TestFontUsage fixed_width_font = new("test-fixedwidth", fixedWidth: true);

        private readonly TestStore fontStore;

        public PositionTextBuilderTest()
        {
            fontStore = new TestStore(
                new GlyphEntry(normal_font, new TestGlyph('カ', x_offset, y_offset, x_advance, width, baseline, height, kerning)),
                new GlyphEntry(normal_font, new TestGlyph('ラ', b_x_offset, b_y_offset, b_x_advance, b_width, b_baseline, b_height, b_kerning)),
                new GlyphEntry(normal_font, new TestGlyph('オ', m_x_offset, m_y_offset, m_x_advance, m_width, m_baseline, m_height, m_kerning)),
                new GlyphEntry(fixed_width_font, new TestGlyph('ケ', x_offset, y_offset, x_advance, width, baseline, height, kerning)),
                new GlyphEntry(fixed_width_font, new TestGlyph('か', b_x_offset, b_y_offset, b_x_advance, b_width, b_baseline, b_height, b_kerning)),
                new GlyphEntry(fixed_width_font, new TestGlyph('ら', m_x_offset, m_y_offset, m_x_advance, m_width, m_baseline, m_height, m_kerning)),
                new GlyphEntry(fixed_width_font, new TestGlyph('お', x_offset, y_offset, x_advance, width, baseline, height, kerning)),
                new GlyphEntry(fixed_width_font, new TestGlyph('け', b_x_offset, b_y_offset, b_x_advance, b_width, b_baseline, b_height, b_kerning)),
                new GlyphEntry(fixed_width_font, new TestGlyph('k', m_x_offset, m_y_offset, m_x_advance, m_width, m_baseline, m_height, m_kerning)),
                new GlyphEntry(fixed_width_font, new TestGlyph('a', x_offset, y_offset, x_advance, width, baseline, height, kerning)),
                new GlyphEntry(fixed_width_font, new TestGlyph('r', b_x_offset, b_y_offset, b_x_advance, b_width, b_baseline, b_height, b_kerning)),
                new GlyphEntry(fixed_width_font, new TestGlyph('o', m_x_offset, m_y_offset, m_x_advance, m_width, m_baseline, m_height, m_kerning)),
                new GlyphEntry(fixed_width_font, new TestGlyph('e', x_offset, y_offset, x_advance, width, baseline, height, kerning))
            );
        }

        private List<TextBuilderGlyph> characterList;

        [SetUp]
        public void SetUp()
        {
            var mainTextBuilder = new TextBuilder(fontStore, normal_font);
            mainTextBuilder.AddText("カラオケ");
            mainTextBuilder.AddText("karaoke");
            characterList = mainTextBuilder.Characters;
        }

        [TestCase('か', true)]
        [TestCase('A', false)]
        public void TestAddPositionTextHasChar(char c, bool equal)
        {
            var builder = new PositionTextBuilder(fontStore, normal_font, normal_font, characterList: characterList);
            builder.AddText(new PositionText
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = c.ToString()
            });

            var character = builder.Characters.LastOrDefault();

            if (equal)
            {
                Assert.AreEqual(character.Character, c);
            }
            else
            {
                // will not render character if not contain this char in font store.
                Assert.AreNotEqual(character.Character, c);
            }
        }

        [TestCase('か', 5.5f, 6.5f)]
        [TestCase('ら', 9.0f, 6.5f)]
        public void TestAddPositionTextPosition(char c, float x, float y)
        {
            var builder = new PositionTextBuilder(fontStore, normal_font, normal_font, characterList: characterList);
            builder.AddText(new PositionText
            {
                StartIndex = 0,
                EndIndex = 1,
                Text = c.ToString()
            });

            var character = builder.Characters.LastOrDefault();
            var topLeftPosition = character.DrawRectangle.TopLeft;
            Assert.AreEqual(topLeftPosition.X, x);
            Assert.AreEqual(topLeftPosition.Y, y);
        }

        private readonly struct TestFontUsage
        {
            private readonly string family;
            private readonly string weight;
            private readonly bool italics;
            private readonly bool fixedWidth;

            public TestFontUsage(string family = null, string weight = null, bool italics = false, bool fixedWidth = false)
            {
                this.family = family;
                this.weight = weight;
                this.italics = italics;
                this.fixedWidth = fixedWidth;
            }

            public static implicit operator FontUsage(TestFontUsage tfu)
                => new(tfu.family, font_size, tfu.weight, tfu.italics, tfu.fixedWidth);
        }

        private class TestStore : ITexturedGlyphLookupStore
        {
            private readonly GlyphEntry[] glyphs;

            public TestStore(params GlyphEntry[] glyphs)
            {
                this.glyphs = glyphs;
            }

            public ITexturedCharacterGlyph Get(string fontName, char character)
            {
                if (string.IsNullOrEmpty(fontName))
                {
                    return glyphs.FirstOrDefault(g => g.Glyph.Character == character).Glyph;
                }

                return glyphs.FirstOrDefault(g => g.Font.FontName == fontName && g.Glyph.Character == character).Glyph;
            }

            public Task<ITexturedCharacterGlyph> GetAsync(string fontName, char character) => throw new System.NotImplementedException();
        }

        private readonly struct GlyphEntry
        {
            public readonly FontUsage Font;
            public readonly ITexturedCharacterGlyph Glyph;

            public GlyphEntry(FontUsage font, ITexturedCharacterGlyph glyph)
            {
                Font = font;
                Glyph = glyph;
            }
        }

        private readonly struct TestGlyph : ITexturedCharacterGlyph
        {
            public Texture Texture => new(1, 1);
            public float XOffset { get; }
            public float YOffset { get; }
            public float XAdvance { get; }
            public float Width { get; }

            public float Baseline { get; }
            public float Height { get; }

            public char Character { get; }

            private readonly float glyphKerning;

            public TestGlyph(char character, float xOffset, float yOffset, float xAdvance, float width, float baseline, float height, float kerning)
            {
                glyphKerning = kerning;
                Character = character;
                XOffset = xOffset;
                YOffset = yOffset;
                XAdvance = xAdvance;
                Width = width;
                Baseline = baseline;
                Height = height;
            }

            public float GetKerning<T>(T lastGlyph)
                where T : ICharacterGlyph
                => glyphKerning;
        }
    }
}
