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

    public partial class KaraokeSpriteText<T> : CompositeDrawable, ISingleShaderBufferedDrawable, IHasRuby, IHasRomaji where T : LyricSpriteText, new()
    {
        private readonly MaskingContainer<T> leftLyricTextContainer;
        private readonly T leftLyricText;

        private readonly MaskingContainer<T> rightLyricTextContainer;
        private readonly T rightLyricText;

        // todo: should have a better way to let user able to customize formats?
        private readonly BufferedDrawNodeSharedData sharedData = new BufferedDrawNodeSharedData(2);

        public IShader TextureShader { get; private set; }
        public IShader RoundedTextureShader { get; private set; }

        public KaraokeSpriteText()
        {
            AutoSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                rightLyricTextContainer = new MaskingContainer<T>
                {
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    MaskingEdges = Edges.Left,
                    Child = rightLyricText = new T
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                    }
                },
                leftLyricTextContainer = new MaskingContainer<T>
                {
                    AutoSizeAxes = Axes.Y,
                    MaskingEdges = Edges.Right,
                    Child = leftLyricText = new T(),
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
                {
                    if (value.Count > 1)
                        throw new NotSupportedException($"{nameof(LyricSpriteText)} does not support more than one shaders now.");

                    shaders.AddRange(value);
                }

                Invalidate(Invalidation.DrawNode);
            }
        }

        public IShader Shader => Shaders.FirstOrDefault();

        public IReadOnlyList<IShader> LeftLyricTextShaders
        {
            get => leftLyricText.Shaders;
            set
            {
                leftLyricText.Shaders = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        public IReadOnlyList<IShader> RightLyricTextShaders
        {
            get => rightLyricText.Shaders;
            set
            {
                rightLyricText.Shaders = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        #endregion

        #region text

        public string Text
        {
            get => leftLyricText.Text;
            set
            {
                leftLyricText.Text = value;
                rightLyricText.Text = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        public IReadOnlyList<PositionText> Rubies
        {
            get => leftLyricText.Rubies;
            set
            {
                leftLyricText.Rubies = value;
                rightLyricText.Rubies = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        public IReadOnlyList<PositionText> Romajies
        {
            get => leftLyricText.Romajies;
            set
            {
                leftLyricText.Romajies = value;
                rightLyricText.Romajies = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        #endregion

        #region font

        public FontUsage Font
        {
            get => leftLyricText.Font;
            set
            {
                leftLyricText.Font = value;
                rightLyricText.Font = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        public FontUsage RubyFont
        {
            get => leftLyricText.RubyFont;
            set
            {
                leftLyricText.RubyFont = value;
                rightLyricText.RubyFont = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        public FontUsage RomajiFont
        {
            get => leftLyricText.RomajiFont;
            set
            {
                leftLyricText.RomajiFont = value;
                rightLyricText.RomajiFont = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        #endregion

        #region style

        public ColourInfo LeftTextColour
        {
            get => leftLyricText.Colour;
            set
            {
                leftLyricText.Colour = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        public ColourInfo RightTextColour
        {
            get => rightLyricText.Colour;
            set
            {
                rightLyricText.Colour = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        public LyricTextAlignment RubyAlignment
        {
            get => leftLyricText.RubyAlignment;
            set
            {
                leftLyricText.RubyAlignment = value;
                rightLyricText.RubyAlignment = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        public LyricTextAlignment RomajiAlignment
        {
            get => leftLyricText.RomajiAlignment;
            set
            {
                leftLyricText.RomajiAlignment = value;
                rightLyricText.RomajiAlignment = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        #endregion

        #region text spacing

        public Vector2 Spacing
        {
            get => leftLyricText.Spacing;
            set
            {
                leftLyricText.Spacing = value;
                rightLyricText.Spacing = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        public Vector2 RubySpacing
        {
            get => leftLyricText.RubySpacing;
            set
            {
                leftLyricText.RubySpacing = value;
                rightLyricText.RubySpacing = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        public Vector2 RomajiSpacing
        {
            get => leftLyricText.RomajiSpacing;
            set
            {
                leftLyricText.RomajiSpacing = value;
                rightLyricText.RomajiSpacing = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        #endregion

        #region margin/padding

        public int RubyMargin
        {
            get => leftLyricText.RubyMargin;
            set
            {
                leftLyricText.RubyMargin = value;
                rightLyricText.RubyMargin = value;

                Invalidate(Invalidation.DrawNode);
            }
        }

        public int RomajiMargin
        {
            get => leftLyricText.RomajiMargin;
            set
            {
                leftLyricText.RomajiMargin = value;
                rightLyricText.RomajiMargin = value;

                Invalidate(Invalidation.DrawNode);
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
                leftLyricText.LifetimeStart = value;
                rightLyricText.LifetimeStart = value;
            }
        }

        public override double LifetimeEnd
        {
            get => base.LifetimeEnd;
            set
            {
                base.LifetimeEnd = value;
                leftLyricText.LifetimeEnd = value;
                rightLyricText.LifetimeEnd = value;
            }
        }

        // TODO : implement
        public bool Continuous { get; set; }

        // TODO : implement
        public KaraokeTextSmartHorizon KaraokeTextSmartHorizon { get; set; }

        public override Vector2 Size
        {
            get => leftLyricText.Size;
            set => throw new InvalidOperationException();
        }

        protected override bool OnInvalidate(Invalidation invalidation, InvalidationSource source)
        {
            var result = base.OnInvalidate(invalidation, source);

            var hasTimeTag = TimeTags != null;
            var hasText = !string.IsNullOrEmpty(Text);
            if (!invalidation.HasFlag(Invalidation.Presence) || !hasTimeTag || !hasText)
                return result;

            Schedule(RefreshStateTransforms);

            return true;
        }

        public virtual void RefreshStateTransforms()
        {
            // todo: IApplicableToCharacterSize should affect padding in the masking container also.

            // set initial width.
            // we should get width from child object because draw width haven't updated.
            var width = leftLyricText.Width;
            leftLyricTextContainer.Width = 0;
            rightLyricTextContainer.Width = width;

            // reset masking transform.
            leftLyricTextContainer.ClearTransforms();
            rightLyricTextContainer.ClearTransforms();

            // filter valid time-tag with order.
            var validTimeTag = TimeTags
                               .Where(x => x.Key.Index >= 0 && x.Key.Index < Text.Length)
                               .OrderBy(x => x.Value).ToArray();

            // get first time-tag relative start time.
            var currentTime = Time.Current;
            var relativeTime = validTimeTag.FirstOrDefault().Value;

            // get transform sequence and set initial delay time.
            var delay = relativeTime - currentTime;
            var leftTransformSequence = leftLyricTextContainer.Delay(delay).ResizeWidthTo(0).Then();
            var rightTransformSequence = rightLyricTextContainer.Delay(delay).ResizeWidthTo(width).Then();

            foreach ((var textIndex, double time) in validTimeTag)
            {
                // calculate position and duration relative to precious time-tag time.
                var position = getCharacterPosition(textIndex);
                var duration = Math.Max(time - relativeTime, 0);

                // apply the position with delay time.
                leftTransformSequence.ResizeWidthTo(position, duration).Then();
                rightTransformSequence.ResizeWidthTo(width - position, duration).Then();

                // save current time-tag time for letting next time-tag able to calculate duration.
                relativeTime = time;
            }
        }

        private float getCharacterPosition(TextIndex index)
            => index.State == TextIndex.IndexState.Start ? leftLyricText.GetTextIndexPosition(index) : rightLyricText.GetTextIndexPosition(index);
    }
}
