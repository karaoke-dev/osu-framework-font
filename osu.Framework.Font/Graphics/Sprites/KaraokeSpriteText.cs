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
        internal const double INTERPOLATION_TIMING = 1;

        private readonly Container<T> leftLyricTextContainer;
        private readonly T leftLyricText;

        private readonly Container<T> rightLyricTextContainer;
        private readonly T rightLyricText;

        // todo: should have a better way to let user able to customize formats?
        private readonly BufferedDrawNodeSharedData sharedData = new BufferedDrawNodeSharedData(2);

        public IShader TextureShader { get; private set; } = null!;
        public IShader RoundedTextureShader { get; private set; } = null!;

        public KaraokeSpriteText()
        {
            AutoSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                rightLyricTextContainer = new Container<T>
                {
                    AutoSizeAxes = Axes.Y,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Masking = true,
                    Child = rightLyricText = new T
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                    }
                },
                leftLyricTextContainer = new Container<T>
                {
                    AutoSizeAxes = Axes.Y,
                    Masking = true,
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

        #region Frame buffer

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

                if (value.Count > 1)
                    throw new NotSupportedException($"{nameof(LyricSpriteText)} does not support more than one shaders now.");

                shaders.AddRange(value);

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

                Invalidate(Invalidation.Layout);
            }
        }

        public IReadOnlyList<IShader> RightLyricTextShaders
        {
            get => rightLyricText.Shaders;
            set
            {
                rightLyricText.Shaders = value;

                Invalidate(Invalidation.Layout);
            }
        }

        #endregion

        #region Text

        public string Text
        {
            get => leftLyricText.Text;
            set
            {
                leftLyricText.Text = value;
                rightLyricText.Text = value;

                Invalidate(Invalidation.Layout);
            }
        }

        public IReadOnlyList<PositionText> Rubies
        {
            get => leftLyricText.Rubies;
            set
            {
                leftLyricText.Rubies = value;
                rightLyricText.Rubies = value;

                Invalidate(Invalidation.Layout);
            }
        }

        public IReadOnlyList<PositionText> Romajies
        {
            get => leftLyricText.Romajies;
            set
            {
                leftLyricText.Romajies = value;
                rightLyricText.Romajies = value;

                Invalidate(Invalidation.Layout);
            }
        }

        #endregion

        #region Font

        public FontUsage Font
        {
            get => leftLyricText.Font;
            set
            {
                leftLyricText.Font = value;
                rightLyricText.Font = value;

                Invalidate(Invalidation.Layout);
            }
        }

        public FontUsage RubyFont
        {
            get => leftLyricText.RubyFont;
            set
            {
                leftLyricText.RubyFont = value;
                rightLyricText.RubyFont = value;

                Invalidate(Invalidation.Layout);
            }
        }

        public FontUsage RomajiFont
        {
            get => leftLyricText.RomajiFont;
            set
            {
                leftLyricText.RomajiFont = value;
                rightLyricText.RomajiFont = value;

                Invalidate(Invalidation.Layout);
            }
        }

        #endregion

        #region Style

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

                Invalidate(Invalidation.Layout);
            }
        }

        public LyricTextAlignment RomajiAlignment
        {
            get => leftLyricText.RomajiAlignment;
            set
            {
                leftLyricText.RomajiAlignment = value;
                rightLyricText.RomajiAlignment = value;

                Invalidate(Invalidation.Layout);
            }
        }

        #endregion

        #region Text spacing

        public Vector2 Spacing
        {
            get => leftLyricText.Spacing;
            set
            {
                leftLyricText.Spacing = value;
                rightLyricText.Spacing = value;

                Invalidate(Invalidation.Layout);
            }
        }

        public Vector2 RubySpacing
        {
            get => leftLyricText.RubySpacing;
            set
            {
                leftLyricText.RubySpacing = value;
                rightLyricText.RubySpacing = value;

                Invalidate(Invalidation.Layout);
            }
        }

        public Vector2 RomajiSpacing
        {
            get => leftLyricText.RomajiSpacing;
            set
            {
                leftLyricText.RomajiSpacing = value;
                rightLyricText.RomajiSpacing = value;

                Invalidate(Invalidation.Layout);
            }
        }

        #endregion

        #region Margin/padding

        public int RubyMargin
        {
            get => leftLyricText.RubyMargin;
            set
            {
                leftLyricText.RubyMargin = value;
                rightLyricText.RubyMargin = value;

                Invalidate(Invalidation.Layout);
            }
        }

        public int RomajiMargin
        {
            get => leftLyricText.RomajiMargin;
            set
            {
                leftLyricText.RomajiMargin = value;
                rightLyricText.RomajiMargin = value;

                Invalidate(Invalidation.Layout);
            }
        }

        #endregion

        private readonly SortedDictionary<double, TextIndex> timeTags = new SortedDictionary<double, TextIndex>();

        public IReadOnlyDictionary<double, TextIndex> TimeTags
        {
            get => timeTags;
            set
            {
                timeTags.Clear();

                foreach (var (timeTag, time) in value)
                {
                    timeTags.Add(timeTag, time);
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

            if (!invalidation.HasFlag(Invalidation.Layout))
                return result;

            Schedule(RefreshStateTransforms);

            return true;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            // Because refresh state only triggered if some property changed.
            // So we should make sure that it will be triggered at least once.
            RefreshStateTransforms();
        }
    }
}
