// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shaders;
using osu.Framework.Layout;
using osu.Framework.Text;
using osu.Framework.Utils;
using osuTK;

namespace osu.Framework.Graphics.Sprites
{
    public partial class LyricSpriteText
    {
        #region Text builder

        /// <summary>
        /// The characters that should be excluded from fixed-width application. Defaults to (".", ",", ":", " ") if null.
        /// </summary>
        protected virtual char[] FixedWidthExcludeCharacters => null;

        /// <summary>
        /// The character to use to calculate the fixed width width. Defaults to 'm'.
        /// </summary>
        protected virtual char FixedWidthReferenceCharacter => 'm';

        /// <summary>
        /// The character to fallback to use if a character glyph lookup failed.
        /// </summary>
        protected virtual char FallbackCharacter => '?';

        private readonly LayoutValue<TextBuilder> textBuilderCache = new LayoutValue<TextBuilder>(Invalidation.DrawSize, InvalidationSource.Parent);
        private readonly LayoutValue<TextBuilder> rubyTextBuilderCache = new LayoutValue<TextBuilder>(Invalidation.DrawSize, InvalidationSource.Parent);
        private readonly LayoutValue<TextBuilder> romajiTextBuilderCache = new LayoutValue<TextBuilder>(Invalidation.DrawSize, InvalidationSource.Parent);

        /// <summary>
        /// Invalidates the current <see cref="TextBuilder"/>, causing a new one to be created next time it's required via <see cref="CreateTextBuilder"/>.
        /// </summary>
        protected void InvalidateTextBuilder()
        {
            textBuilderCache.Invalidate();
            rubyTextBuilderCache.Invalidate();
            romajiTextBuilderCache.Invalidate();
        }

        /// <summary>
        /// Creates a <see cref="TextBuilder"/> to generate the character layout for this <see cref="LyricSpriteText"/>.
        /// </summary>
        /// <param name="store">The <see cref="ITexturedGlyphLookupStore"/> where characters should be retrieved from.</param>
        /// <returns>The <see cref="TextBuilder"/>.</returns>
        protected virtual TextBuilder CreateTextBuilder(ITexturedGlyphLookupStore store)
        {
            var excludeCharacters = FixedWidthExcludeCharacters ?? default_never_fixed_width_characters;

            var rubyHeight = ReserveRubyHeight || Rubies.Any() ? RubyFont.Size : 0;
            var romajiHeight = ReserveRomajiHeight || Romajies.Any() ? RomajiFont.Size : 0;
            var startOffset = new Vector2(Padding.Left, Padding.Top + rubyHeight);
            var spacing = Spacing + new Vector2(0, rubyHeight + romajiHeight);

            float builderMaxWidth = requiresAutoSizedWidth
                ? MaxWidth
                : ApplyRelativeAxes(RelativeSizeAxes, new Vector2(Math.Min(MaxWidth, base.Width), base.Height), FillMode).X - Padding.Right;

            if (AllowMultiline)
            {
                return new MultilineTextBuilder(store, Font, builderMaxWidth, UseFullGlyphHeight, startOffset, spacing, charactersBacking,
                    excludeCharacters, FallbackCharacter, FixedWidthReferenceCharacter);
            }

            if (Truncate)
            {
                return new TruncatingTextBuilder(store, Font, builderMaxWidth, ellipsisString, UseFullGlyphHeight, startOffset, spacing, charactersBacking,
                    excludeCharacters, FallbackCharacter, FixedWidthReferenceCharacter);
            }

            return new TextBuilder(store, Font, builderMaxWidth, UseFullGlyphHeight, startOffset, spacing, charactersBacking,
                excludeCharacters, FallbackCharacter, FixedWidthReferenceCharacter);
        }

        protected virtual TextBuilder CreateRubyTextBuilder(ITexturedGlyphLookupStore store)
        {
            const int builder_max_width = int.MaxValue;
            var excludeCharacters = FixedWidthExcludeCharacters ?? default_never_fixed_width_characters;

            return new TextBuilder(store, RubyFont, builder_max_width, UseFullGlyphHeight,
                new Vector2(), rubySpacing, null, excludeCharacters, FallbackCharacter, FixedWidthReferenceCharacter);
        }

        protected virtual TextBuilder CreateRomajiTextBuilder(ITexturedGlyphLookupStore store)
        {
            const int builder_max_width = int.MaxValue;
            var excludeCharacters = FixedWidthExcludeCharacters ?? default_never_fixed_width_characters;

            return new TextBuilder(store, RomajiFont, builder_max_width, UseFullGlyphHeight,
                new Vector2(), romajiSpacing, null, excludeCharacters, FallbackCharacter, FixedWidthReferenceCharacter);
        }

        private TextBuilder getTextBuilder()
        {
            if (!textBuilderCache.IsValid)
                textBuilderCache.Value = CreateTextBuilder(store);

            return textBuilderCache.Value;
        }

        private TextBuilder getRubyTextBuilder()
        {
            if (!rubyTextBuilderCache.IsValid)
                rubyTextBuilderCache.Value = CreateRubyTextBuilder(store);

            return rubyTextBuilderCache.Value;
        }

        private TextBuilder getRomajiTextBuilder()
        {
            if (!romajiTextBuilderCache.IsValid)
                romajiTextBuilderCache.Value = CreateRomajiTextBuilder(store);

            return romajiTextBuilderCache.Value;
        }

        public float LineBaseHeight
        {
            get
            {
                computeCharacters();
                return textBuilderCache.Value.LineBaseHeight;
            }
        }

        #endregion

        #region Characters

        private readonly LayoutValue charactersCache = new LayoutValue(Invalidation.DrawSize | Invalidation.Presence, InvalidationSource.Parent);

        /// <summary>
        /// Glyph list to be passed to <see cref="TextBuilder"/>.
        /// </summary>
        private readonly List<TextBuilderGlyph> charactersBacking = new List<TextBuilderGlyph>();

        /// <summary>
        /// The characters in local space.
        /// </summary>
        private IReadOnlyList<TextBuilderGlyph> characters
        {
            get
            {
                computeCharacters();
                return charactersBacking;
            }
        }

        /// <summary>
        /// Compute character textures and positions.
        /// </summary>
        private void computeCharacters()
        {
            // Note : this feature can only use in osu-framework
            // if (LoadState >= LoadState.Loaded)
            //     ThreadSafety.EnsureUpdateThread();

            if (store == null)
                return;

            if (charactersCache.IsValid)
                return;

            charactersBacking.Clear();

            // Todo: Re-enable this assert after autosize is split into two passes.
            // Debug.Assert(!isComputingCharacters, "Cyclic invocation of computeCharacters()!");

            Vector2 textBounds = Vector2.Zero;

            try
            {
                if (string.IsNullOrEmpty(displayedText))
                    return;

                TextBuilder textBuilder = getTextBuilder();

                textBuilder.Reset();
                textBuilder.AddText(displayedText);
                textBounds = textBuilder.Bounds;

                var fixedRubies = getFixedPositionTexts(rubies, displayedText);
                var fixedRomajies = getFixedPositionTexts(romajies, displayedText);

                if (fixedRubies.Any())
                {
                    var rubyTextBuilder = getRubyTextBuilder();
                    fixedRubies.ForEach(x => rubyTextBuilder.AddText(x));
                }

                if (fixedRomajies.Any())
                {
                    var romajiTextBuilder = getRomajiTextBuilder();
                    fixedRomajies.ForEach(x => romajiTextBuilder.AddText(x));
                }
            }
            finally
            {
                if (requiresAutoSizedWidth)
                    base.Width = textBounds.X + Padding.Right;

                if (requiresAutoSizedHeight)
                {
                    var romajiHeight = ReserveRomajiHeight || Romajies.Any() ? RomajiFont.Size : 0;
                    base.Height = textBounds.Y + romajiHeight + Padding.Bottom;
                }

                base.Width = Math.Min(base.Width, MaxWidth);

                charactersCache.Validate();
            }

            static List<PositionText> getFixedPositionTexts(IEnumerable<PositionText> positionTexts, string lyricText)
                => positionTexts
                   .Where(x => !string.IsNullOrEmpty(x.Text))
                   .Select(x => GetFixedPositionText(x, lyricText))
                   .ToList();
        }

        internal static PositionText GetFixedPositionText(PositionText positionText, string lyricText)
        {
            var startIndex = Math.Clamp(positionText.StartIndex, 0, lyricText.Length);
            var endIndex = Math.Clamp(positionText.EndIndex, 0, lyricText.Length);
            return new PositionText(positionText.Text, Math.Min(startIndex, endIndex), Math.Max(startIndex, endIndex));
        }

        #endregion

        #region Screen space characters

        private readonly LayoutValue parentScreenSpaceCache = new LayoutValue(Invalidation.DrawSize | Invalidation.Presence | Invalidation.DrawInfo, InvalidationSource.Parent);
        private readonly LayoutValue localScreenSpaceCache = new LayoutValue(Invalidation.MiscGeometry, InvalidationSource.Self);

        private readonly List<ScreenSpaceCharacterPart> screenSpaceCharactersBacking = new List<ScreenSpaceCharacterPart>();

        /// <summary>
        /// The characters in screen space. These are ready to be drawn.
        /// </summary>
        private IEnumerable<ScreenSpaceCharacterPart> screenSpaceCharacters
        {
            get
            {
                computeScreenSpaceCharacters();
                return screenSpaceCharactersBacking;
            }
        }

        private void computeScreenSpaceCharacters()
        {
            if (!parentScreenSpaceCache.IsValid)
            {
                localScreenSpaceCache.Invalidate();
                parentScreenSpaceCache.Validate();
            }

            if (localScreenSpaceCache.IsValid)
                return;

            screenSpaceCharactersBacking.Clear();

            Vector2 inflationAmount = DrawInfo.MatrixInverse.ExtractScale().Xy;

            foreach (var character in characters)
            {
                screenSpaceCharactersBacking.Add(new ScreenSpaceCharacterPart
                {
                    DrawQuad = ToScreenSpace(character.DrawRectangle.Inflate(inflationAmount)),
                    InflationPercentage = Vector2.Divide(inflationAmount, character.DrawRectangle.Size),
                    Texture = character.Texture
                });
            }

            localScreenSpaceCache.Validate();
        }

        #endregion

        #region Character position

        public float GetTextIndexXPosition(TextIndex index)
        {
            var computedRectangle = GetCharacterDrawRectangle(index.Index);
            return index.State == TextIndex.IndexState.Start ? computedRectangle.Left : computedRectangle.Right;
        }

        public RectangleF GetCharacterDrawRectangle(int index, bool drawSizeOnly = false)
        {
            int charIndex = Math.Clamp(index, 0, Text.Length - 1);
            if (charIndex != index)
                throw new ArgumentOutOfRangeException(nameof(index));

            var character = characters[charIndex];
            var drawRectangle = TextBuilderGlyphUtils.GetCharacterRectangle(character, drawSizeOnly);
            return getComputeCharacterDrawRectangle(drawRectangle);
        }

        public RectangleF GetRubyTagDrawRectangle(PositionText rubyTag, bool drawSizeOnly = false)
        {
            int rubyIndex = Rubies.ToList().IndexOf(rubyTag);
            if (rubyIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(rubyIndex));

            int startCharacterIndex = Text.Length + skinIndex(Rubies, rubyIndex);
            int count = rubyTag.Text.Length;
            var drawRectangle = characters.ToList()
                                          .GetRange(startCharacterIndex, count)
                                          .Select(x => TextBuilderGlyphUtils.GetCharacterRectangle(x, drawSizeOnly))
                                          .Aggregate(RectangleF.Union);
            return getComputeCharacterDrawRectangle(drawRectangle);
        }

        public RectangleF GetRomajiTagDrawRectangle(PositionText romajiTag, bool drawSizeOnly = false)
        {
            int romajiIndex = Romajies.ToList().IndexOf(romajiTag);
            if (romajiIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(romajiIndex));

            int startCharacterIndex = Text.Length + skinIndex(Rubies, Rubies.Count) + skinIndex(Romajies, romajiIndex);
            int count = romajiTag.Text.Length;
            var drawRectangle = characters.ToList()
                                          .GetRange(startCharacterIndex, count)
                                          .Select(x => TextBuilderGlyphUtils.GetCharacterRectangle(x, drawSizeOnly))
                                          .Aggregate(RectangleF.Union);
            return getComputeCharacterDrawRectangle(drawRectangle);
        }

        private int skinIndex(IEnumerable<PositionText> positionTexts, int endIndex)
            => positionTexts.Where((_, i) => i < endIndex).Sum(x => x.Text.Length);

        private RectangleF getComputeCharacterDrawRectangle(RectangleF originalCharacterDrawRectangle)
        {
            // combine the rectangle to get the max value.
            return Shaders.OfType<IApplicableToCharacterSize>()
                          .Select(x => x.ComputeCharacterDrawRectangle(originalCharacterDrawRectangle))
                          .Aggregate(originalCharacterDrawRectangle, RectangleF.Union);
        }

        #endregion
    }
}
