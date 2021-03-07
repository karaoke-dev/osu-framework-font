// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace osu.Framework.Graphics.Sprites
{
    public class KaraokeSpriteText : KaraokeSpriteText<LyricSpriteText>
    {
    }

    public class KaraokeSpriteText<T> : CompositeDrawable, IHasRuby, IHasRomaji where T : LyricSpriteText, new()
    {
        private readonly Container frontLyricTextContainer;
        private readonly T frontLyricText;

        private readonly Container backLyricTextContainer;
        private readonly T backLyricText;

        public KaraokeSpriteText()
        {
            AutoSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                backLyricTextContainer = new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Masking = true,
                    Child = backLyricText = new T()
                },
                frontLyricTextContainer = new Container
                {
                    AutoSizeAxes = Axes.Y,
                    Masking = true,
                    Child = frontLyricText = new T()
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

        private IReadOnlyDictionary<TextIndex, double> timeTags = new Dictionary<TextIndex, double>();

        public IReadOnlyDictionary<TextIndex, double> TimeTags
        {
            get => timeTags;
            set
            {
                // Check timetag's index and time is ordered.
                var orderedByIndexDictionary = value?.OrderBy(x => x.Key).ToDictionary(d => d.Key, d => d.Value);
                var orderedByTimeDictionary = value?.OrderBy(x => x.Value).ToDictionary(d => d.Key, d => d.Value);
                if (!orderedByIndexDictionary.SequenceEqual(orderedByTimeDictionary))
                    throw new Exception($"{nameof(value)} should be ordered.");

                timeTags = orderedByIndexDictionary;
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

                    maskWidth = percentage.Status == DisplayPercentage.DisplayStatus.Exceed ? backLyricTextContainer.Width : 0;
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

        public float GetPercentageWidth(TextIndex startIndex, TextIndex endIndex, float percentage = 0)
            => backLyricText.GetPercentageWidth(startIndex, endIndex, percentage);

        private DisplayPercentage getPercentageByTime(double time)
        {
            var availableTimeTags = TimeTags.Where(x => x.Key.Index >= 0 && x.Key.Index < Text.Length)
                                            .ToDictionary(d => d.Key, d => d.Value);

            if (!availableTimeTags.Any())
                return new DisplayPercentage(DisplayPercentage.DisplayStatus.NotYet);

            // Less than start time
            if (time < availableTimeTags.FirstOrDefault().Value)
                return new DisplayPercentage(DisplayPercentage.DisplayStatus.NotYet);

            // More the end time
            if (time > availableTimeTags.LastOrDefault().Value)
                return new DisplayPercentage(DisplayPercentage.DisplayStatus.Exceed);

            var startTagTime = availableTimeTags.LastOrDefault(x => x.Value < time);
            var endTagTime = availableTimeTags.FirstOrDefault(x => x.Value > time);

            var percentage = Math.Min((time - startTagTime.Value) / (endTagTime.Value - startTagTime.Value), 1);
            return new DisplayPercentage(startTagTime.Key, endTagTime.Key, (float)percentage);
        }

        internal readonly struct DisplayPercentage
        {
            public DisplayPercentage(DisplayStatus status)
            {
                StartIndex = EndIndex = new TextIndex();

                if (status == DisplayStatus.Exceed)
                    StartIndex = EndIndex = new TextIndex(int.MaxValue);
                else if (status == DisplayStatus.NotYet)
                    StartIndex = EndIndex = new TextIndex(int.MinValue);
                else if (status == DisplayStatus.Available)
                    throw new ArgumentOutOfRangeException($"Cannot accept type {nameof(DisplayStatus.Available)}");

                Status = status;
                TextPercentage = 0;
            }

            public DisplayPercentage(TextIndex startTime, TextIndex endTime, float textPercentage)
            {
                StartIndex = startTime;
                EndIndex = endTime;
                TextPercentage = textPercentage;
                Status = DisplayStatus.Available;
            }

            public TextIndex StartIndex { get; }

            public TextIndex EndIndex { get; }

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
