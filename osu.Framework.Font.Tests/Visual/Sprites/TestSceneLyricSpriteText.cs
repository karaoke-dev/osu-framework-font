// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Tests.Visual.Sprites
{
    public class TestSceneLyricSpriteText : TestScene
    {
        public TestSceneLyricSpriteText()
        {
            AddLabel("Text");
            AddStep("Text", () => SetContents(() => createLyricSpriteText("ローマ字")));
            AddStep("Text and ruby", () => SetContents(() => createLyricSpriteText("ローマ字", new[] { null, null, null, "じ" })));
            AddStep("Text and romaji", () => SetContents(() => createLyricSpriteText("ローマ字", null, new[] { "ro", null, "ma", "ji" })));
            AddStep("Text and ruby and romaji", () => SetContents(() => createLyricSpriteText("ローマ字", new[] { null, null, null, "じ" }, new[] { "ro", null, "ma", "ji" })));

            AddStep("Text and ruby", () => SetContents(() => createLyricSpriteText("ローマ字", "ろーまじ")));
            AddStep("Text and romaji", () => SetContents(() => createLyricSpriteText("ローマ字", null, "Romaji")));
            AddStep("Text and ruby and romaji", () => SetContents(() => createLyricSpriteText("ローマ字", "ろーまじ", "Romaji")));

            AddLabel("Layout");
            AddStep("Ruby margin", () => SetContents(() => createLyricSpriteText("スペース", 10), 500, x => x.RubyMargin = 20));
            AddStep("Romaji margin", () => SetContents(() => createLyricSpriteText("スペース", null, 10), 500, x => x.RomajiMargin = 20));
            AddStep("Ruby and romaji margin", 
                () => SetContents(() => createLyricSpriteText("スペース", 10, 10), 500, x => {
                x.RomajiMargin = 20;
                x.RomajiMargin = 20;
            }));
            AddStep("Ruby spacing", () => SetContents(() => createLyricSpriteText("スペース", new Vector2(1)), 500, x => x.RubySpacing = new Vector2(10)));
            AddStep("Romaji spacing", () => SetContents(() => createLyricSpriteText("スペース", null, new Vector2(1)), 500, x => x.RomajiSpacing = new Vector2(10)));
            AddStep("Ruby and romaji spacing",
                () => SetContents(() => createLyricSpriteText("スペース", new Vector2(1), new Vector2(10)), 500, x => {
                    x.RubySpacing = new Vector2(10);
                    x.RomajiSpacing = new Vector2(10);
                }));
        }

        public void SetContents(Func<LyricSpriteText> creationFunction)
        {
            Child = creationFunction();
        }

        public void SetContents(Func<LyricSpriteText> creationFunction, double delay, Action<LyricSpriteText> thenDo)
        {
            var lyricLine = creationFunction();
            Child = lyricLine;

            new Task(async delegate
            {
                await Task.Delay(TimeSpan.FromSeconds(delay / 1000));
                thenDo?.Invoke(lyricLine);
            }).Start();
        }

        private LyricSpriteText createLyricSpriteText(string text, string[] rubyStrings, string[] romajiStrings = null)
        {
            var rubies = new List<PositionText>();

            for (int i = 0; i < rubyStrings?.Length; i++)
            {
                rubies.Add(new PositionText(rubyStrings[i], i, i + 1));
            }

            var romajies = new List<PositionText>();

            for (int i = 0; i < romajiStrings?.Length; i++)
            {
                romajies.Add(new PositionText(romajiStrings[i], i, i + 1));
            }

            return createLyricSpriteText(text, rubies, romajies);
        }

        private LyricSpriteText createLyricSpriteText(string text, string ruby, string romaji = null)
        {
            const int star_index = 0;
            var endIndex = text.Length;

            var rubies = new List<PositionText>
            {
                new PositionText(ruby, star_index, endIndex)
            };

            var romajies = new List<PositionText>
            {
                new PositionText(romaji, star_index, endIndex)
            };

            return createLyricSpriteText(text, rubies, romajies);
        }

        private LyricSpriteText createLyricSpriteText(string text, int? rubyMargin = null, int? RomajiMargin = null)
        {
            var lyricSpriteText = createLyricSpriteText(text, "ルビ", "ローマ字");
            if (rubyMargin != null)
                lyricSpriteText.RubyMargin = rubyMargin.Value;

            if (RomajiMargin != null)
                lyricSpriteText.RomajiMargin = RomajiMargin.Value;

            return lyricSpriteText;
        }

        private LyricSpriteText createLyricSpriteText(string text, Vector2? rubySpacing, Vector2? RomajiSpacing = null)
        {
            var lyricSpriteText = createLyricSpriteText(text, "ルビ", "ローマ字");
            if (rubySpacing != null)
                lyricSpriteText.RubySpacing = rubySpacing.Value;

            if (RomajiSpacing != null)
                lyricSpriteText.RomajiSpacing = RomajiSpacing.Value;

            return lyricSpriteText;
        }

        private LyricSpriteText createLyricSpriteText(string text, List<PositionText> rubies, List<PositionText> romajies)
        {
            return new LyricSpriteText
            {
                Text = text,
                Rubies = rubies?.ToArray(),
                Romajies = romajies?.ToArray(),
                Margin = new MarginPadding(30),
                Shadow = true,
                ShadowOffset = new Vector2(3),
                Outline = true,
                OutlineRadius = 3f,
                ShadowTexture = new SolidTexture { SolidColor = Color4.Red },
                TextTexture = new SolidTexture { SolidColor = Color4.White }
            };
        }
    }
}
