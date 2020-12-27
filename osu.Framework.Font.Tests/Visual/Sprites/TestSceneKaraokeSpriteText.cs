// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Tests.Visual.Sprites
{
    public class TestSceneKaraokeSpriteText : TestScene
    {
        public TestSceneKaraokeSpriteText()
        {
            AddLabel("Test time tag");
            AddStep("Default time tag", () =>
            {
                var startTime = Time.Current;
                Child = createCustomizeTimeTagKaraokeText(new Dictionary<TimeTagIndex, double>
                {
                    { new TimeTagIndex(0), startTime + 500 },
                    { new TimeTagIndex(1), startTime + 600 },
                    { new TimeTagIndex(2), startTime + 1000 },
                    { new TimeTagIndex(3), startTime + 1500 },
                    { new TimeTagIndex(4), startTime + 2000 },
                });
            });
            AddStep("Time tag with end state", () =>
            {
                var startTime = Time.Current;
                Child = createCustomizeTimeTagKaraokeText(new Dictionary<TimeTagIndex, double>
                {
                    // カ
                    { new TimeTagIndex(0), startTime + 0 },
                    { new TimeTagIndex(0, TimeTagIndex.IndexState.End), startTime + 100 },
                    // ラ
                    { new TimeTagIndex(1), startTime + 1000 },
                    { new TimeTagIndex(1, TimeTagIndex.IndexState.End), startTime + 1100 },
                    // オ
                    { new TimeTagIndex(2), startTime + 2000 },
                    { new TimeTagIndex(2, TimeTagIndex.IndexState.End), startTime + 2100 },
                    // ケ
                    { new TimeTagIndex(3), startTime + 3000 },
                    { new TimeTagIndex(3, TimeTagIndex.IndexState.End), startTime + 3100 },
                    // !
                    { new TimeTagIndex(4), startTime + 4000 },
                    { new TimeTagIndex(4, TimeTagIndex.IndexState.End), startTime + 4100 },
                });
            });
            AddStep("Time tag with wrong order", () =>
            {
                var startTime = Time.Current;
                Child = createCustomizeTimeTagKaraokeText(new Dictionary<TimeTagIndex, double>
                {
                    { new TimeTagIndex(4), startTime + 2000 },
                    { new TimeTagIndex(3), startTime + 1500 },
                    { new TimeTagIndex(2), startTime + 1000 },
                    { new TimeTagIndex(1), startTime + 600 },
                    { new TimeTagIndex(0), startTime + 500 },
                });
            });
            AddStep("Time tag with out of range", () =>
            {
                var startTime = Time.Current;
                Child = createCustomizeTimeTagKaraokeText(new Dictionary<TimeTagIndex, double>
                {
                    { new TimeTagIndex(-1), startTime + 0 },
                    { new TimeTagIndex(0), startTime + 500 },
                    { new TimeTagIndex(1), startTime + 600 },
                    { new TimeTagIndex(2), startTime + 1000 },
                    { new TimeTagIndex(3), startTime + 1500 },
                    { new TimeTagIndex(4), startTime + 2000 },
                    { new TimeTagIndex(8), startTime + 2500 },
                });
            });
        }

        private Drawable createCustomizeTimeTagKaraokeText(Dictionary<TimeTagIndex, double> timeTags)
        {
            return new KaraokeSpriteText
            {
                Text = "カラオケ！",
                TimeTags = timeTags,
                Rubies = new[]
                {
                    new PositionText
                    {
                        StartIndex = 0,
                        EndIndex = 1,
                        Text = "か"
                    },
                    new PositionText
                    {
                        StartIndex = 2,
                        EndIndex = 3,
                        Text = "お"
                    }
                },
                Romajies = new[]
                {
                    new PositionText
                    {
                        StartIndex = 1,
                        EndIndex = 2,
                        Text = "ra"
                    },
                    new PositionText
                    {
                        StartIndex = 3,
                        EndIndex = 4,
                        Text = "ke"
                    }
                },
                Shadow = true,
                ShadowOffset = new Vector2(3),
                FrontTextShadowTexture = new SolidTexture { SolidColor = Color4.Green },
                FrontTextTexture = new SolidTexture { SolidColor = Color4.LightBlue },
                BackTextShadowTexture = new SolidTexture { SolidColor = Color4.Red },
            };
        }
    }
}
