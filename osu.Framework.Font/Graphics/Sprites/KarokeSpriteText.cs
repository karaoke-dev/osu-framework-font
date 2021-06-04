// Copyright (c) andy840119 <andy840119@gmail.com>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Graphics.Containers;
using osu.Framework.Layout;
using osu.Framework.Utils;
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

        public IReadOnlyDictionary<TextIndex, double> TimeTags { get; set; } = new Dictionary<TextIndex, double>();

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

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            var result = base.OnInvalidate(invalidation, source);

            var hasTimeTag = TimeTags != null;
            var hasText = !string.IsNullOrEmpty(Text);
            if (!invalidation.HasFlag(Invalidation.Presence) || !hasTimeTag || !hasText)
                return result;

            // reset masking transform.
            frontLyricTextContainer.ClearTransforms();

            // process time-tag should in the text-range
            var characters = frontLyricText.Characters;

            foreach (var (textIndex, time) in TimeTags)
            {
                // text-index should be in the range.
                var validTextIndex = TextIndexUtils.Clamp(textIndex, 0, Text.Length - 1);
                var characterRectangle = characters[validTextIndex.Index].DrawRectangle;
                var position = validTextIndex.State == TextIndex.IndexState.Start ? characterRectangle.Left : characterRectangle.Right;
                var duration = Math.Max(time - Time.Current, 0);
                frontLyricTextContainer.ResizeWidthTo(position, duration);
            }

            return true;
        }
    }
}
