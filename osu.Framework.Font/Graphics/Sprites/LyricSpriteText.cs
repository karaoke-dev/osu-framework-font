// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.IO.Stores;
using osu.Framework.Localisation;
using osu.Framework.Utils;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Graphics.Sprites;

/// <summary>
/// A container for simple text rendering purposes. If more complex text rendering is required, use <see cref="TextFlowContainer"/> instead.
/// </summary>
public partial class LyricSpriteText : Drawable, IMultiShaderBufferedDrawable, IHasLineBaseHeight, IHasFilterTerms, IFillFlowContainer, IHasCurrentValue<string>, IHasTopText, IHasBottomText
{
    private const float default_text_size = 48;
    private static readonly char[] default_never_fixed_width_characters = { '.', ',', ':', ' ' };

    // todo: should have a better way to let user able to customize formats?
    private readonly MultiShaderBufferedDrawNodeSharedData sharedData = new();

    [Resolved]
    private FontStore store { get; set; } = null!;

    public IShader TextureShader { get; private set; } = null!;

    public LyricSpriteText()
    {
        Font = new FontUsage(null, default_text_size);
        current.BindValueChanged(t => Text = t.NewValue);

        AddLayout(charactersCache);
        AddLayout(parentScreenSpaceCache);
        AddLayout(localScreenSpaceCache);
        AddLayout(textBuilderCache);
        AddLayout(topTextBuilderCache);
        AddLayout(bottomTextBuilderCache);
    }

    [BackgroundDependencyLoader]
    private void load(ShaderManager shaderManager)
    {
        TextureShader = shaderManager.Load(VertexShaderDescriptor.TEXTURE_2, FragmentShaderDescriptor.TEXTURE);

        // Pre-cache the characters in the texture store
        foreach (var character in displayedText)
        {
            _ = store.Get(font.FontName, character) ?? store.Get(null, character);
        }
    }

    #region Frame buffer

    public DrawColourInfo? FrameBufferDrawColour => new DrawColourInfo(Color4.White);

    private Color4 backgroundColour = new(0, 0, 0, 0);

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

    private readonly List<ICustomizedShader> shaders = new();

    public IReadOnlyList<ICustomizedShader> Shaders
    {
        get => shaders;
        set
        {
            shaders.Clear();

            shaders.AddRange(value);

            Invalidate();
        }
    }

    #endregion

    #region Text

    private string text = string.Empty;

    /// <summary>
    /// Gets or sets the text to be displayed.
    /// </summary>
    public string Text
    {
        get => text;
        set
        {
            if (text == value)
                return;

            text = value;

            current.Value = text;

            invalidate(true);
        }
    }

    private readonly BindableWithCurrent<string> current = new();

    public Bindable<string> Current
    {
        get => current.Current;
        set => current.Current = value;
    }

    private string displayedText => Text;

    private readonly List<PositionText> topTexts = new();

    /// <summary>
    /// Gets or sets the top text to be displayed.
    /// </summary>
    public IReadOnlyList<PositionText> TopTexts
    {
        get => topTexts;
        set
        {
            topTexts.Clear();
            topTexts.AddRange(value);

            invalidate(true);
        }
    }

    private readonly List<PositionText> bottomTexts = new();

    /// <summary>
    /// Gets or sets the bottom text to be displayed.
    /// </summary>
    public IReadOnlyList<PositionText> BottomTexts
    {
        get => bottomTexts;
        set
        {
            bottomTexts.Clear();
            bottomTexts.AddRange(value);

            invalidate(true);
        }
    }

    #endregion

    #region Font

    private FontUsage font = FontUsage.Default;

    /// <summary>
    /// Contains information on the font used to display the text.
    /// </summary>
    public FontUsage Font
    {
        get => font;
        set
        {
            font = value;

            invalidate(true, true);
        }
    }

    private FontUsage topTextFont = FontUsage.Default;

    /// <summary>
    /// Contains information on the font used to display the text.
    /// </summary>
    public FontUsage TopTextFont
    {
        get => topTextFont;
        set
        {
            topTextFont = value;

            invalidate(true, true);
        }
    }

    private FontUsage bottomTextFont = FontUsage.Default;

    /// <summary>
    /// Contains information on the font used to display the text.
    /// </summary>
    public FontUsage BottomTextFont
    {
        get => bottomTextFont;
        set
        {
            bottomTextFont = value;

            invalidate(true, true);
        }
    }

    #endregion

    #region Style

    private bool allowMultiline = true;

    /// <summary>
    /// True if the text should be wrapped if it gets too wide. Note that \n does NOT cause a line break. If you need explicit line breaks, use <see cref="TextFlowContainer"/> instead.
    /// </summary>
    /// <remarks>
    /// If enabled, <see cref="Truncate"/> will be disabled.
    /// </remarks>
    public bool AllowMultiline
    {
        get => allowMultiline;
        set
        {
            if (allowMultiline == value)
                return;

            if (value)
                Truncate = false;

            allowMultiline = value;

            invalidate(true, true);
        }
    }

    private bool useFullGlyphHeight = true;

    /// <summary>
    /// True if the <see cref="LyricSpriteText"/>'s vertical size should be equal to <see cref="FontUsage.Size"/>  (the full height) or precisely the size of used characters.
    /// Set to false to allow better centering of individual characters/numerals/etc.
    /// </summary>
    public bool UseFullGlyphHeight
    {
        get => useFullGlyphHeight;
        set
        {
            if (useFullGlyphHeight == value)
                return;

            useFullGlyphHeight = value;

            invalidate(true, true);
        }
    }

    private LyricTextAlignment topTextAlignment;

    /// <summary>
    /// Gets or sets the top text alignment.
    /// </summary>
    public LyricTextAlignment TopTextAlignment
    {
        get => topTextAlignment;
        set
        {
            if (topTextAlignment == value)
                return;

            topTextAlignment = value;
            invalidate(true, true);
        }
    }

    private LyricTextAlignment bottomTextAlignment;

    /// <summary>
    /// Gets or sets the bottom text alignment.
    /// </summary>
    public LyricTextAlignment BottomTextAlignment
    {
        get => bottomTextAlignment;
        set
        {
            if (bottomTextAlignment == value)
                return;

            bottomTextAlignment = value;
            invalidate(true, true);
        }
    }

    private bool truncate;

    /// <summary>
    /// If true, text should be truncated when it exceeds the <see cref="Drawable.DrawWidth"/> of this <see cref="LyricSpriteText"/>.
    /// </summary>
    /// <remarks>
    /// Has no effect if no <see cref="Width"/> or custom sizing is set.
    /// If enabled, <see cref="AllowMultiline"/> will be disabled.
    /// </remarks>
    public bool Truncate
    {
        get => truncate;
        set
        {
            if (truncate == value) return;

            if (value)
                AllowMultiline = false;

            truncate = value;
            invalidate(true, true);
        }
    }

    private string ellipsisString = "…";

    /// <summary>
    /// When <see cref="Truncate"/> is enabled, this decides what string is used to signify that truncation has occured.
    /// Defaults to "…".
    /// </summary>
    public string EllipsisString
    {
        get => ellipsisString;
        set
        {
            if (ellipsisString == value) return;

            ellipsisString = value;
            invalidate(true, true);
        }
    }

    #endregion

    #region Size

    private bool requiresAutoSizedWidth => explicitWidth == null && (RelativeSizeAxes & Axes.X) == 0;

    private bool requiresAutoSizedHeight => explicitHeight == null && (RelativeSizeAxes & Axes.Y) == 0;

    private float? explicitWidth;

    /// <summary>
    /// Gets or sets the width of this <see cref="LyricSpriteText"/>. The <see cref="LyricSpriteText"/> will maintain this width when set.
    /// </summary>
    public override float Width
    {
        get
        {
            if (requiresAutoSizedWidth)
                computeCharacters();
            return base.Width;
        }
        set
        {
            if (explicitWidth == value)
                return;

            base.Width = value;
            explicitWidth = value;

            invalidate(true, true);
        }
    }

    private float maxWidth = float.PositiveInfinity;

    /// <summary>
    /// The maximum width of this <see cref="LyricSpriteText"/>. Affects both auto and fixed sizing modes.
    /// </summary>
    /// <remarks>
    /// This becomes a relative value if this <see cref="LyricSpriteText"/> is relatively-sized on the X-axis.
    /// </remarks>
    public float MaxWidth
    {
        get => maxWidth;
        set
        {
            if (maxWidth == value)
                return;

            maxWidth = value;
            invalidate(true, true);
        }
    }

    private float? explicitHeight;

    /// <summary>
    /// Gets or sets the height of this <see cref="LyricSpriteText"/>. The <see cref="LyricSpriteText"/> will maintain this height when set.
    /// </summary>
    public override float Height
    {
        get
        {
            if (requiresAutoSizedHeight)
                computeCharacters();
            return base.Height;
        }
        set
        {
            if (explicitHeight == value)
                return;

            base.Height = value;
            explicitHeight = value;

            invalidate(true, true);
        }
    }

    /// <summary>
    /// Gets or sets the size of this <see cref="LyricSpriteText"/>. The <see cref="LyricSpriteText"/> will maintain this size when set.
    /// </summary>
    public override Vector2 Size
    {
        get
        {
            if (requiresAutoSizedWidth || requiresAutoSizedHeight)
                computeCharacters();
            return base.Size;
        }
        set
        {
            Width = value.X;
            Height = value.Y;
        }
    }

    #endregion

    #region Text spacing

    private Vector2 spacing;

    /// <summary>
    /// Gets or sets the spacing between characters of this <see cref="LyricSpriteText"/>.
    /// </summary>
    public Vector2 Spacing
    {
        get => spacing;
        set
        {
            if (spacing == value)
                return;

            spacing = value;

            invalidate(true, true);
        }
    }

    private Vector2 topTextSpacing;

    /// <summary>
    /// Gets or sets the spacing between characters of top text.
    /// </summary>
    public Vector2 TopTextSpacing
    {
        get => topTextSpacing;
        set
        {
            if (topTextSpacing == value)
                return;

            topTextSpacing = value;

            invalidate(true, true);
        }
    }

    private Vector2 bottomTextSpacing;

    /// <summary>
    /// Gets or sets the spacing between characters of bottom text.
    /// </summary>
    public Vector2 BottomTextSpacing
    {
        get => bottomTextSpacing;
        set
        {
            if (bottomTextSpacing == value)
                return;

            bottomTextSpacing = value;

            invalidate(true, true);
        }
    }

    #endregion

    #region Margin/padding

    private MarginPadding padding;

    /// <summary>
    /// Shrinks the space which may be occupied by characters of this <see cref="LyricSpriteText"/> by the specified amount on each side.
    /// </summary>
    public MarginPadding Padding
    {
        get => padding;
        set
        {
            if (padding.Equals(value))
                return;

            if (!Validation.IsFinite(value)) throw new ArgumentException($@"{nameof(Padding)} must be finite, but is {value}.");

            padding = value;

            invalidate(true, true);
        }
    }

    private int topTextMargin;

    /// <summary>
    /// Shrinks the space between top and main text.
    /// </summary>
    public int TopTextMargin
    {
        get => topTextMargin;
        set
        {
            if (topTextMargin == value)
                return;

            topTextMargin = value;

            invalidate(true, true);
        }
    }

    private int bottomTextMargin;

    /// <summary>
    /// Shrinks the space between bottom and main text.
    /// </summary>
    public int BottomTextMargin
    {
        get => bottomTextMargin;
        set
        {
            if (bottomTextMargin == value)
                return;

            bottomTextMargin = value;

            invalidate(true, true);
        }
    }

    private bool reserveTopTextHeight;

    /// <summary>
    /// Reserve top text height even contains no text.
    /// </summary>
    public bool ReserveTopTextHeight
    {
        get => reserveTopTextHeight;
        set
        {
            if (reserveTopTextHeight == value)
                return;

            reserveTopTextHeight = value;

            invalidate(true, true);
        }
    }

    private bool reserveBottomTextHeight;

    /// <summary>
    /// Reserve top text height even contains no text.
    /// </summary>
    public bool ReserveBottomTextHeight
    {
        get => reserveBottomTextHeight;
        set
        {
            if (reserveBottomTextHeight == value)
                return;

            reserveBottomTextHeight = value;

            invalidate(true, true);
        }
    }

    #endregion

    public override bool IsPresent => base.IsPresent && (AlwaysPresent || !string.IsNullOrEmpty(displayedText));

    #region Invalidation

    private void invalidate(bool characters = false, bool textBuilder = false)
    {
        if (characters)
            charactersCache.Invalidate();

        if (textBuilder)
            InvalidateTextBuilder();

        parentScreenSpaceCache.Invalidate();
        localScreenSpaceCache.Invalidate();

        Invalidate(Invalidation.DrawNode);
    }

    #endregion

    public override string ToString() => $@"""{displayedText}"" " + base.ToString();

    public IEnumerable<LocalisableString> FilterTerms => new LocalisableString(displayedText).Yield();
}
