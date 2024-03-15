// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using NUnit.Framework;
using osu.Framework.Font.Tests.Helper;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace osu.Framework.Font.Tests.Visual.Sprites;

public partial class TestSceneLyricSpriteText : FrameworkTestScene
{
    [TestCase("karaoke", null, null)]
    [TestCase("カラオケ", new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }, null)]
    [TestCase("－－オケ", new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }, null)]
    [TestCase("－－－－", new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" }, null)]
    [TestCase("カラオケ－－", new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け", "[4]:け", "[5]:－" }, null)]
    [TestCase("カラオケ－－カ", new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け", "[4]:け", "[5]:－", "[6]:－" }, null)]
    [TestCase("カラオケ", null, new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke" })]
    [TestCase("－－オケ", null, new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke" })]
    [TestCase("－－－－", null, new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke" })]
    [TestCase("カラオケ－－", null, new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke", "[4]:ke", "[5]:－" })]
    [TestCase("カラオケ－－カ", null, new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke", "[4]:ke", "[5]:－", "[6]:－" })]
    public void TestText(string text, string[] rubyTags, string[] romajiTags)
    {
        AddStep("Create lyric", () => setContents(() => new LyricSpriteText
        {
            Text = text,
            TopTexts = TestCaseTagHelper.ParsePositionTexts(rubyTags),
            BottomTexts = TestCaseTagHelper.ParsePositionTexts(romajiTags),
        }));
    }

    [TestCase(48, 24, 24)]
    [TestCase(48, 10, 24)]
    [TestCase(48, 24, 10)]
    public void TestFont(int mainFontSize, int rubyFontSize, int romajiFontSize)
    {
        AddStep("Create lyric", () => setContents(() => new DefaultLyricSpriteText
        {
            Font = new FontUsage(null, mainFontSize),
            TopTextFont = new FontUsage(null, rubyFontSize),
            BottomTextFont = new FontUsage(null, romajiFontSize),
        }));
    }

    [TestCase(200)]
    [TestCase(100)]
    [TestCase(50)]
    public void TestMultiline(int width)
    {
        AddStep("Create lyric with text", () => setContents(() => new DefaultLyricSpriteText(false, false)
        {
            AllowMultiline = true,
            Width = width
        }));

        AddStep("Create lyric with ruby", () => setContents(() => new DefaultLyricSpriteText
        {
            AllowMultiline = true,
            Width = width
        }));

        AddStep("Create lyric with romaji", () => setContents(() => new DefaultLyricSpriteText(false)
        {
            AllowMultiline = true,
            Width = width
        }));

        AddStep("Create lyric with ruby and romaji.", () => setContents(() => new DefaultLyricSpriteText
        {
            AllowMultiline = true,
            Width = width
        }));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestUseFullGlyphHeight(bool use)
    {
        AddStep("Create lyric with text", () => setContents(() => new DefaultLyricSpriteText(false, false)
        {
            UseFullGlyphHeight = use
        }));

        AddStep("Create lyric with ruby", () => setContents(() => new DefaultLyricSpriteText
        {
            UseFullGlyphHeight = use
        }));

        AddStep("Create lyric with romaji", () => setContents(() => new DefaultLyricSpriteText(false)
        {
            UseFullGlyphHeight = use
        }));

        AddStep("Create lyric with ruby and romaji.", () => setContents(() => new DefaultLyricSpriteText
        {
            UseFullGlyphHeight = use
        }));
    }

    [Ignore("This feature not implement in lyric text.")]
    public void TestTexture()
    {
        // todo : waiting this feature implement.
    }

    [TestCase(LyricTextAlignment.Auto, LyricTextAlignment.Auto)]
    [TestCase(LyricTextAlignment.Auto, LyricTextAlignment.Center)]
    [TestCase(LyricTextAlignment.Auto, LyricTextAlignment.EqualSpace)]
    [TestCase(LyricTextAlignment.Center, LyricTextAlignment.Auto)]
    [TestCase(LyricTextAlignment.Center, LyricTextAlignment.Center)]
    [TestCase(LyricTextAlignment.Center, LyricTextAlignment.EqualSpace)]
    [TestCase(LyricTextAlignment.EqualSpace, LyricTextAlignment.Auto)]
    [TestCase(LyricTextAlignment.EqualSpace, LyricTextAlignment.Center)]
    [TestCase(LyricTextAlignment.EqualSpace, LyricTextAlignment.EqualSpace)]
    public void TestAlignment(LyricTextAlignment rubyAlignment, LyricTextAlignment romajiAlignment)
    {
        // todo : this feature is not implemented.
        AddStep("Create lyric", () => setContents(() => new DefaultLyricSpriteText
        {
            TopTextAlignment = rubyAlignment,
            BottomTextAlignment = romajiAlignment
        }));
    }

    [TestCase(false, "")]
    [TestCase(true, "…")]
    [TestCase(true, "...")]
    [TestCase(true, "___")]
    public void TestTruncate(bool truncate, string ellipsisString)
    {
        AddStep("Create lyric", () => setContents(() => new DefaultLyricSpriteText
        {
            AllowMultiline = false,
            Truncate = truncate,
            EllipsisString = ellipsisString
        }));

        // Note : it will not hide the text if set truncate to false, and extra text will be force rendered.
        AddStep("Create lyric", () => setContents(() => new DefaultLyricSpriteText
        {
            AllowMultiline = false,
            Truncate = truncate,
            EllipsisString = ellipsisString,
            Width = 100,
        }));
    }

    [Ignore("This property not need to be tested.")]
    public void TestSizing()
    {
    }

    [TestCase(null, null, null)]
    [TestCase("(10,0)", null, null)]
    [TestCase(null, "(10,0)", null)]
    [TestCase(null, null, "(10,0)")]
    public void TestSpacing(string spacing, string rubySpacing, string romajiSpacing)
    {
        // todo : will cause display weird in ruby or romaji.
        AddStep("Create lyric", () => setContents(() => new DefaultLyricSpriteText
        {
            Spacing = TestCaseVectorHelper.ParseVector2(spacing),
            TopTextSpacing = TestCaseVectorHelper.ParseVector2(rubySpacing),
            BottomTextSpacing = TestCaseVectorHelper.ParseVector2(romajiSpacing),
        }));
    }

    [TestCase(0, 0)]
    [TestCase(0, 20)]
    [TestCase(20, 0)]
    public void TestMarginPadding(int rubyMargin, int romajiMargin)
    {
        AddStep("Create lyric", () => setContents(() => new DefaultLyricSpriteText
        {
            TopTextMargin = rubyMargin,
            BottomTextMargin = romajiMargin,
        }));
    }

    [TestCase(false, false, false)]
    [TestCase(true, true, false)]
    [TestCase(false, true, false)]
    [TestCase(true, true, false)]
    [TestCase(false, false, true)]
    [TestCase(true, true, true)]
    [TestCase(false, true, true)]
    [TestCase(true, true, true)]
    public void TestReserveHeight(bool reserveRubyHeight, bool reserveRomajiHeight, bool multiLine)
    {
        AddStep("Create lyric with text", () => setContents(() => new DefaultLyricSpriteText(false, false)
        {
            Width = multiLine ? 50 : 200,
            AllowMultiline = multiLine,
            ReserveTopTextHeight = reserveRubyHeight,
            ReserveBottomTextHeight = reserveRomajiHeight,
        }));

        AddStep("Create lyric with ruby", () => setContents(() => new DefaultLyricSpriteText
        {
            Width = multiLine ? 50 : 200,
            AllowMultiline = multiLine,
            ReserveTopTextHeight = reserveRubyHeight,
            ReserveBottomTextHeight = reserveRomajiHeight,
        }));

        AddStep("Create lyric with romaji", () => setContents(() => new DefaultLyricSpriteText(false)
        {
            Width = multiLine ? 50 : 200,
            AllowMultiline = multiLine,
            ReserveTopTextHeight = reserveRubyHeight,
            ReserveBottomTextHeight = reserveRomajiHeight,
        }));

        AddStep("Create lyric with ruby and romaji.", () => setContents(() => new DefaultLyricSpriteText
        {
            Width = multiLine ? 50 : 200,
            AllowMultiline = multiLine,
            ReserveTopTextHeight = reserveRubyHeight,
            ReserveBottomTextHeight = reserveRomajiHeight,
        }));
    }

    private void setContents(Func<LyricSpriteText> creationFunction)
    {
        Child = creationFunction().With(x => x.Scale = new Vector2(2));
    }

    internal partial class DefaultLyricSpriteText : LyricSpriteText
    {
        public DefaultLyricSpriteText(bool ruby = true, bool romaji = true)
        {
            Text = "カラオケ";
            Font = FontUsage.Default;

            if (ruby)
            {
                TopTexts = TestCaseTagHelper.ParsePositionTexts(new[] { "[0]:か", "[1]:ら", "[2]:お", "[3]:け" });
            }

            if (romaji)
            {
                BottomTexts = TestCaseTagHelper.ParsePositionTexts(new[] { "[0]:ka", "[1]:ra", "[2]:o", "[3]:ke" });
            }
        }
    }
}
