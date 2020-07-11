// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Tests.Visual.Sprites
{
    public class TestSceneKarakeSpriteText : TestScene
    {
        public TestSceneKarakeSpriteText()
        {
            AddLabel("Test timetag");
            AddStep("Default timetag", () =>
            {
                var starttime = Time.Current;
                Child = createCustomizeTimeTagKaroakeText(new Dictionary<TimeTagIndex, double>
                {
                    { new TimeTagIndex(0), starttime + 500 },
                    { new TimeTagIndex(1), starttime + 600 },
                    { new TimeTagIndex(2), starttime + 1000 },
                    { new TimeTagIndex(3), starttime + 1500 },
                    { new TimeTagIndex(4), starttime + 2000 },
                });
            });
            AddStep("Timetag with end state", () =>
            {
                var starttime = Time.Current;
                Child = createCustomizeTimeTagKaroakeText(new Dictionary<TimeTagIndex, double>
                {
                    // カ
                    { new TimeTagIndex(0), starttime + 0 },
                    { new TimeTagIndex(1, TimeTagIndex.IndexState.End), starttime + 100 },
                    // ラ
                    { new TimeTagIndex(1), starttime + 1000 },
                    { new TimeTagIndex(2, TimeTagIndex.IndexState.End), starttime + 1100 },
                    // オ
                    { new TimeTagIndex(2), starttime + 2000 },
                    { new TimeTagIndex(3, TimeTagIndex.IndexState.End), starttime + 2100 },
                    // ケ
                    { new TimeTagIndex(3), starttime + 3000 },
                    { new TimeTagIndex(4, TimeTagIndex.IndexState.End), starttime + 3100 },
                    // !
                    { new TimeTagIndex(4), starttime + 4000 },
                    { new TimeTagIndex(5, TimeTagIndex.IndexState.End), starttime + 4100 },
                });
            });
            AddStep("Timetag with wrong order", () =>
            {
                var starttime = Time.Current;
                Child = createCustomizeTimeTagKaroakeText(new Dictionary<TimeTagIndex, double>
                {
                    { new TimeTagIndex(4), starttime + 2000 },
                    { new TimeTagIndex(3), starttime + 1500 },
                    { new TimeTagIndex(2), starttime + 1000 },
                    { new TimeTagIndex(1), starttime + 600 },
                    { new TimeTagIndex(0), starttime + 500 },
                });
            });
            AddStep("Timetag with out of range", () =>
            {
                var starttime = Time.Current;
                Child = createCustomizeTimeTagKaroakeText(new Dictionary<TimeTagIndex, double>
                {
                    { new TimeTagIndex(-1), starttime + 0 },
                    { new TimeTagIndex(0), starttime + 500 },
                    { new TimeTagIndex(1), starttime + 600 },
                    { new TimeTagIndex(2), starttime + 1000 },
                    { new TimeTagIndex(3), starttime + 1500 },
                    { new TimeTagIndex(4), starttime + 2000 },
                    { new TimeTagIndex(8), starttime + 2500 },
                });
            });
        }

        private Drawable createCustomizeTimeTagKaroakeText(Dictionary<TimeTagIndex, double> timeTags)
        {
            return new KarakeSpriteText
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
                Outline = true,
                OutlineRadius = 3f,
                FrontTextShadowTexture = new SolidTexture { SolidColor = Color4.Green },
                FrontTextTexture = new SolidTexture { SolidColor = Color4.LightBlue },
                BackTextShadowTexture = new SolidTexture { SolidColor = Color4.Red },
            };
        }
    }
}
