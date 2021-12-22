// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Layout;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Sprites
{
    public class KaraokeSpriteText : KaraokeSpriteText<LyricSpriteText>
    {
    }

    public partial class KaraokeSpriteText<T> : CompositeDrawable, IMultiShaderBufferedDrawable, IHasRuby, IHasRomaji where T : LyricSpriteText, new()
    {
        private readonly MaskingContainer<T> frontLyricTextContainer;
        private readonly T frontLyricText;

        private readonly MaskingContainer<T> backLyricTextContainer;
        private readonly T backLyricText;

        // todo: should have a better way to let user able to customize formats?
        private readonly MultiShaderBufferedDrawNodeSharedData sharedData = new MultiShaderBufferedDrawNodeSharedData();

        public IShader TextureShader { get; private set; }
        public IShader RoundedTextureShader { get; private set; }

        public KaraokeSpriteText()
        {
            AutoSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                backLyricTextContainer = new MaskingContainer<T>
                {
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    MaskingEdges = Edges.Left,
                    Child = backLyricText = new T
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                    }
                },
                frontLyricTextContainer = new MaskingContainer<T>
                {
                    AutoSizeAxes = Axes.Y,
                    MaskingEdges = Edges.Right,
                    Child = frontLyricText = new T(),
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(ShaderManager shaderManager)
        {
            TextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);
            RoundedTextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE_ROUNDED);
        }

        #region frame buffer

        public DrawColourInfo? FrameBufferDrawColour => base.DrawColourInfo;

        // Children should not receive the true colour to avoid colour doubling when the frame-buffers are rendered to the back-buffer.
        public override DrawColourInfo DrawColourInfo
        {
            get
            {
                // Todo: This is incorrect.
                var blending = Blending;
                blending.ApplyDefaultToInherited();

                return new DrawColourInfo(Color4.White, blending);
            }
        }

        private Color4 backgroundColour = new Color4(0, 0, 0, 0);

        /// <summary>
        /// The background colour of the framebuffer. Transparent black by default.
        /// </summary>
        public Color4 BackgroundColour
        {
            get => backgroundColour;
            set
            {
                if (backgroundColour == value)
                    return;

                backgroundColour = value;
                Invalidate(Invalidation.DrawNode);
            }
        }

        private Vector2 frameBufferScale = Vector2.One;

        public Vector2 FrameBufferScale
        {
            get => frameBufferScale;
            set
            {
                if (frameBufferScale == value)
                    return;

                frameBufferScale = value;
                Invalidate(Invalidation.DrawNode);
            }
        }

        #endregion

        #region Shader

        private readonly List<IShader> shaders = new List<IShader>();

        public IReadOnlyList<IShader> Shaders
        {
            get => shaders;
            set
            {
                shaders.Clear();

                if (value != null)
                    shaders.AddRange(value);

                Invalidate(Invalidation.DrawNode);
            }
        }

        public IReadOnlyList<IShader> LeftLyricTextShaders
        {
            get => frontLyricText.Shaders;
            set
            {
                frontLyricText.Shaders = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        public IReadOnlyList<IShader> RightLyricTextShaders
        {
            get => backLyricText.Shaders;
            set
            {
                backLyricText.Shaders = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        #endregion

        #region text

        public string Text
        {
            get => frontLyricText.Text;
            set
            {
                frontLyricText.Text = value;
                backLyricText.Text = value;
            }
        }

        public IReadOnlyList<PositionText> Rubies
        {
            get => frontLyricText.Rubies;
            set
            {
                frontLyricText.Rubies = value;
                backLyricText.Rubies = value;
            }
        }

        public IReadOnlyList<PositionText> Romajies
        {
            get => frontLyricText.Romajies;
            set
            {
                frontLyricText.Romajies = value;
                backLyricText.Romajies = value;
            }
        }

        #endregion

        #region font

        public FontUsage Font
        {
            get => frontLyricText.Font;
            set
            {
                frontLyricText.Font = value;
                backLyricText.Font = value;
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

        #endregion

        #region style

        public ColourInfo LeftTextColour
        {
            get => frontLyricText.Colour;
            set => frontLyricText.Colour = value;
        }

        public ColourInfo RightTextColour
        {
            get => backLyricText.Colour;
            set => backLyricText.Colour = value;
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

        #endregion

        #region text spacing

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

        #endregion

        #region margin/padding

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

        #endregion

        private readonly Dictionary<TextIndex, double> timeTags = new Dictionary<TextIndex, double>();

        public IReadOnlyDictionary<TextIndex, double> TimeTags
        {
            get => timeTags;
            set
            {
                timeTags.Clear();

                if (value != null)
                {
                    foreach (var (timeTag, time) in value)
                    {
                        timeTags.Add(timeTag, time);
                    }
                }

                Invalidate(Invalidation.Layout);
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

        // TODO : implement
        public bool Continuous { get; set; }

        // TODO : implement
        public KaraokeTextSmartHorizon KaraokeTextSmartHorizon { get; set; }

        public override Vector2 Size
        {
            get => frontLyricText.Size;
            set => throw new InvalidOperationException();
        }

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            var result = base.OnInvalidate(invalidation, source);

            var hasTimeTag = TimeTags != null;
            var hasText = !string.IsNullOrEmpty(Text);
            if (!invalidation.HasFlag(Invalidation.Presence) || !hasTimeTag || !hasText)
                return result;

            // set initial width.
            frontLyricTextContainer.Width = 0;
            backLyricTextContainer.Width = DrawWidth;

            // reset masking transform.
            frontLyricTextContainer.ClearTransforms();
            backLyricTextContainer.ClearTransforms();

            // filter valid time-tag with order.
            var characters = frontLyricText.Characters;
            var validTimeTag = TimeTags
                               .Where(x => x.Key.Index >= 0 && x.Key.Index < Text.Length)
                               .OrderBy(x => x.Value).ToArray();

            // get first time-tag relative start time.
            var currentTime = Time.Current;
            var relativeTime = validTimeTag.FirstOrDefault().Value;

            // should use absolute time to process time-tags.
            using (frontLyricTextContainer.BeginAbsoluteSequence(currentTime))
            using (frontLyricTextContainer.BeginAbsoluteSequence(currentTime))
            {
                // get transform sequence and set initial delay time.
                var frontTransformSequence = frontLyricTextContainer.Delay(relativeTime - currentTime).Then();
                var backTransformSequence = backLyricTextContainer.Delay(relativeTime - currentTime).Then();

                foreach (var (textIndex, time) in validTimeTag)
                {
                    // calculate position and duration relative to precious time-tag time.
                    var characterRectangle = characters[textIndex.Index].DrawRectangle;
                    var position = textIndex.State == TextIndex.IndexState.Start ? characterRectangle.Left : characterRectangle.Right;
                    var duration = Math.Max(time - relativeTime, 0);

                    // apply the position with delay time.
                    frontTransformSequence.ResizeWidthTo(position, duration).Then();
                    backTransformSequence.ResizeWidthTo(DrawWidth - position, duration).Then();

                    // save current time-tag time for letting next time-tag able to calculate duration.
                    relativeTime = time;
                }
            }

            return true;
        }

        public override bool RemoveCompletedTransforms => false;
    }
}
