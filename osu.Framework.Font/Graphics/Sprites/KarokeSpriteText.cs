// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace osu.Framework.Graphics.Sprites
{
    public class KarakeSpriteText : CompositeDrawable, IHasRuby, IHasRomaji
    {
        private readonly Container frontLyricTextContainer;
        private readonly LyricSpriteText frontLyricText;

        private readonly Container backlyricTextContainer;
        private readonly LyricSpriteText backLyricText;

        public KarakeSpriteText()
        {
            AutoSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                backlyricTextContainer = new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Masking = true,
                    Child = backLyricText = new LyricSpriteText()
                },
                frontLyricTextContainer = new Container
                {
                    AutoSizeAxes = Axes.Y,
                    Masking = true,
                    Child = frontLyricText = new LyricSpriteText()
                }
            };
        }

        public string Text
        {
            get => frontLyricText.Text;
            set
            {
                frontLyricText.Text = value;
                backLyricText.Text = value;
            }
        }

        public FontUsage Font
        {
            get => frontLyricText.Font;
            set
            {
                frontLyricText.Font = value;
                backLyricText.Font = value;
            }
        }

        public PositionText[] Rubies
        {
            get => frontLyricText.Rubies;
            set
            {
                frontLyricText.Rubies = value;
                backLyricText.Rubies = value;
            }
        }

        public PositionText[] Romajies
        {
            get => frontLyricText.Romajies;
            set
            {
                frontLyricText.Romajies = value;
                backLyricText.Romajies = value;
            }
        }

        public FontUsage RubyFont
        {
            get => frontLyricText.RubyFont;
            set
            {
                frontLyricText.RubyFont = value;
                backLyricText.RubyFont = value;
            }
        }

        public FontUsage RomajiFont
        {
            get => frontLyricText.RomajiFont;
            set
            {
                frontLyricText.RomajiFont = value;
                backLyricText.RomajiFont = value;
            }
        }

        public int RubyMargin
        {
            get => frontLyricText.RubyMargin;
            set
            {
                frontLyricText.RubyMargin = value;
                backLyricText.RubyMargin = value;
            }
        }

        public int RomajiMargin
        {
            get => frontLyricText.RomajiMargin;
            set
            {
                frontLyricText.RomajiMargin = value;
                backLyricText.RomajiMargin = value;
            }
        }

        public Vector2 Spacing
        {
            get => frontLyricText.Spacing;
            set
            {
                frontLyricText.Spacing = value;
                backLyricText.Spacing = value;
            }
        }

        public Vector2 RubySpacing
        {
            get => frontLyricText.RubySpacing;
            set
            {
                frontLyricText.RubySpacing = value;
                backLyricText.RubySpacing = value;
            }
        }

        public Vector2 RomajiSpacing
        {
            get => frontLyricText.RomajiSpacing;
            set
            {
                frontLyricText.RomajiSpacing = value;
                backLyricText.RomajiSpacing = value;
            }
        }

        private IReadOnlyDictionary<TimeTagIndex, double> timetags = new Dictionary<TimeTagIndex, double>();

        public IReadOnlyDictionary<TimeTagIndex, double> TimeTags
        {
            get => timetags;
            set
            {
                // Check timetag's index and time is ordered.
                var orderedByIndexDictionary = value?.OrderBy(x => x.Key).ToDictionary(d => d.Key, d => d.Value);
                var orderedByTimeDictionary = value?.OrderBy(x => x.Value).ToDictionary(d => d.Key, d => d.Value);
                if (!orderedByIndexDictionary.SequenceEqual(orderedByTimeDictionary))
                    throw new Exception($"{nameof(value)} should be ordered.");

                timetags = orderedByIndexDictionary;
            }
        }

        public ILyricTexture FrontTextTexture
        {
            get => frontLyricText.TextTexture;
            set => frontLyricText.TextTexture = value;
        }

        public ILyricTexture FrontBorderTexture
        {
            get => frontLyricText.BorderTexture;
            set => frontLyricText.BorderTexture = value;
        }

        public ILyricTexture FrontTextShadowTexture
        {
            get => frontLyricText.ShadowTexture;
            set => frontLyricText.ShadowTexture = value;
        }

        public ILyricTexture BackTextTexture
        {
            get => backLyricText.TextTexture;
            set => backLyricText.TextTexture = value;
        }

        public ILyricTexture BackBorderTexture
        {
            get => backLyricText.BorderTexture;
            set => backLyricText.BorderTexture = value;
        }

        public ILyricTexture BackTextShadowTexture
        {
            get => backLyricText.ShadowTexture;
            set => backLyricText.ShadowTexture = value;
        }

        public bool Shadow
        {
            get => frontLyricText.Shadow;
            set
            {
                frontLyricText.Shadow = value;
                backLyricText.Shadow = value;
            }
        }

        public Vector2 ShadowOffset
        {
            get => frontLyricText.ShadowOffset;
            set
            {
                frontLyricText.ShadowOffset = value;
                backLyricText.ShadowOffset = value;
            }
        }

        public override double LifetimeStart
        {
            get => base.LifetimeStart;
            set
            {
                base.LifetimeStart = value;
                frontLyricText.LifetimeStart = value;
                backLyricText.LifetimeStart = value;
            }
        }

        public override double LifetimeEnd
        {
            get => base.LifetimeEnd;
            set
            {
                base.LifetimeEnd = value;
                frontLyricText.LifetimeEnd = value;
                backLyricText.LifetimeEnd = value;
            }
        }

        public LyricTextAlignment RubyAlignment
        {
            get => frontLyricText.RubyAlignment;
            set
            {
                frontLyricText.RubyAlignment = value;
                backLyricText.RubyAlignment = value;
            }
        }

        public LyricTextAlignment RomajiAlignment
        {
            get => frontLyricText.RomajiAlignment;
            set
            {
                frontLyricText.RomajiAlignment = value;
                backLyricText.RomajiAlignment = value;
            }
        }

        public float BorderRadius
        {
            get => frontLyricText.BorderRadius;
            set
            {
                frontLyricText.BorderRadius = value;
                backLyricText.BorderRadius = value;
            }
        }

        public bool Border
        {
            get => frontLyricText.Border;
            set
            {
                frontLyricText.Border = value;
                backLyricText.Border = value;
            }
        }

        // TODO : implement
        public bool Continuous { get; set; }

        // TODO : implement
        public KaraokeTextSmartHorizon KaraokeTextSmartHorizon { get; set; }

        protected override void Update()
        {
            if (Time.Current < LifetimeStart || Time.Current > LifetimeEnd)
                return;

            updateMask();
            base.Update();
        }

        private bool resetAfterOutOfRange;

        private void updateMask()
        {
            var percentage = getPercentageByTime(Time.Current);
            if (percentage.Status != DisplayPercentage.DisplayStatus.Available && resetAfterOutOfRange)
                return;

            float maskWidth;

            switch (percentage.Status)
            {
                case DisplayPercentage.DisplayStatus.Exceed:
                case DisplayPercentage.DisplayStatus.NotYet:
                    resetAfterOutOfRange = true;

                    maskWidth = percentage.Status == DisplayPercentage.DisplayStatus.Exceed ? backlyricTextContainer.Width : 0;
                    break;

                default:
                    resetAfterOutOfRange = false;

                    // Calculate mask width
                    maskWidth = GetPercentageWidth(percentage.StartIndex, percentage.EndIndex, percentage.TextPercentage);
                    break;
            }

            // Update front karaoke text's width
            frontLyricTextContainer.Width = maskWidth;
        }

        public float GetPercentageWidth(int startIndex, int endIndex, float percentage = 0) 
            => backLyricText.GetPercentageWidth(startIndex, endIndex, percentage);

        private DisplayPercentage getPercentageByTime(double time)
        {
            var availableTimetags = TimeTags.Where(x => x.Key.Index >= 0 && x.Key.Index <= Text.Length)
                    .ToDictionary(d => d.Key, d => d.Value);

            if (!availableTimetags.Any())
                return new DisplayPercentage(DisplayPercentage.DisplayStatus.NotYet);

            // Less than start time
            if (time < availableTimetags.FirstOrDefault().Value)
                return new DisplayPercentage(DisplayPercentage.DisplayStatus.NotYet);

            // More the end time
            if (time > availableTimetags.LastOrDefault().Value)
                return new DisplayPercentage(DisplayPercentage.DisplayStatus.Exceed);

            var startTagTime = availableTimetags.LastOrDefault(x => x.Value < time);
            var endTagTime = availableTimetags.FirstOrDefault(x => x.Value > time);

            var percentage = Math.Min((time - startTagTime.Value) / (endTagTime.Value - startTagTime.Value), 1);
            return new DisplayPercentage(startTagTime.Key.Index, endTagTime.Key.Index, (float)percentage);
        }

        internal readonly struct DisplayPercentage
        {
            public DisplayPercentage(DisplayStatus status)
            {
                StartIndex = EndIndex = 0;

                if (status == DisplayStatus.Exceed)
                    StartIndex = EndIndex = int.MaxValue;
                else if (status == DisplayStatus.NotYet)
                    StartIndex = EndIndex = int.MinValue;
                else if (status == DisplayStatus.Available)
                    throw new ArgumentOutOfRangeException($"Cannot accept type {nameof(DisplayStatus.Available)}");

                Status = status;
                TextPercentage = 0;
            }

            public DisplayPercentage(int startTime, int endTime, float textPercentage)
            {
                StartIndex = startTime;
                EndIndex = endTime;
                TextPercentage = textPercentage;
                Status = DisplayStatus.Available;
            }

            public int StartIndex { get; }

            public int EndIndex { get; }

            public float TextPercentage { get; }

            public DisplayStatus Status { get; }

            public enum DisplayStatus
            {
                Available,

                Exceed,

                NotYet
            }
        }
    }
}
