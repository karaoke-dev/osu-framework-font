// Copyright (c) karaoke.dev <contact@karaoke.dev>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Utils;

namespace osu.Framework.Graphics.Sprites;

public partial class KaraokeSpriteText<T>
{
    private bool interpolateTimeTags = true;

    /// <summary>
    /// Will add extra time-tag for let the transformers become better.
    /// </summary>
    public bool InterpolateTimeTags
    {
        get => interpolateTimeTags;
        set
        {
            if (interpolateTimeTags == value)
                return;

            interpolateTimeTags = value;

            Invalidate(Invalidation.Layout);
        }
    }

    private bool filterDuplicatedTimeTags = true;

    /// <summary>
    /// Will add extra time-tag for let the transformers become better.
    /// </summary>
    public bool FilterDuplicatedTimeTags
    {
        get => filterDuplicatedTimeTags;
        set
        {
            if (filterDuplicatedTimeTags == value)
                return;

            filterDuplicatedTimeTags = value;

            Invalidate(Invalidation.Layout);
        }
    }

    public virtual void RefreshStateTransforms()
    {
        // reset masking transform.
        leftLyricTextContainer.ClearTransforms();
        rightLyricTextContainer.ClearTransforms();

        // filter valid time-tag with order.
        var validTimeTag = getValidTimeTags();

        // not initialize if no text.
        if (string.IsNullOrEmpty(Text))
            return;

        // set initial width.
        // we should get width from child object because draw width haven't updated.
        var width = leftLyricText.Width;
        var startPosition = getTextIndexPosition(new TextIndex());
        var endPosition = width - startPosition;
        leftLyricTextContainer.Width = startPosition;
        rightLyricTextContainer.Width = endPosition;

        // not initialize if no time-tag or text.
        if (!validTimeTag.Any())
            return;

        // get first time-tag relative start time.
        var currentTime = Time.Current;
        var relativeTime = validTimeTag.FirstOrDefault().Key;

        // get transform sequence and set initial delay time.
        var delay = relativeTime - currentTime;
        var leftTransformSequence = leftLyricTextContainer.Delay(delay).ResizeWidthTo(startPosition).Then();
        var rightTransformSequence = rightLyricTextContainer.Delay(delay).ResizeWidthTo(endPosition).Then();

        foreach ((double time, var textIndex) in validTimeTag)
        {
            // calculate position and duration relative to precious time-tag time.
            // todo: deal with the case if got the duplicated time-tag.
            var position = getTextIndexPosition(textIndex);
            var duration = Math.Max(time - relativeTime, 0);

            // apply the position with delay time.
            leftTransformSequence.ResizeWidthTo(position, duration).Then();
            rightTransformSequence.ResizeWidthTo(width - position, duration).Then();

            // save current time-tag time for letting next time-tag able to calculate duration.
            relativeTime = time;
        }
    }

    private IReadOnlyDictionary<double, TextIndex> getValidTimeTags()
    {
        var validTimeTags = GetInTheRangeTimeTags(TimeTags, Text);

        if (FilterDuplicatedTimeTags)
        {
            validTimeTags = GetNonDuplicatedTimeTags(validTimeTags);
        }

        if (InterpolateTimeTags)
        {
            validTimeTags = GetInterpolatedTimeTags(validTimeTags);
        }

        return validTimeTags;
    }

    internal static IReadOnlyDictionary<double, TextIndex> GetInTheRangeTimeTags(IReadOnlyDictionary<double, TextIndex> timeTags, string text)
        => timeTags
           .Where(x => x.Value.Index >= 0 && x.Value.Index < text.Length)
           .ToDictionary(k => k.Key, v => v.Value);

    internal static IReadOnlyDictionary<double, TextIndex> GetNonDuplicatedTimeTags(IReadOnlyDictionary<double, TextIndex> timeTags)
    {
        var orderedTimeTags = timeTags.OrderBy(x => x.Key);
        return orderedTimeTags.Aggregate(new Dictionary<double, TextIndex>(), (collections, lastTimeTag) =>
        {
            if (collections.Any() && collections.LastOrDefault().Value == lastTimeTag.Value)
                return collections;

            collections.Add(lastTimeTag.Key, lastTimeTag.Value);
            return collections;
        });
    }

    internal static IReadOnlyDictionary<double, TextIndex> GetInterpolatedTimeTags(IReadOnlyDictionary<double, TextIndex> timeTags)
    {
        var orderedTimeTags = timeTags.OrderBy(x => x.Key);
        return orderedTimeTags.Aggregate(new Dictionary<double, TextIndex>(), (collections, lastTimeTag) =>
        {
            if (collections.Count == 0)
            {
                collections.Add(lastTimeTag.Key, lastTimeTag.Value);
                return collections;
            }

            foreach (var (time, textIndex) in getInterpolatedTimeTagBetweenTwoTimeTag(collections.LastOrDefault(), lastTimeTag))
            {
                collections.Add(time, textIndex);
            }

            collections.Add(lastTimeTag.Key, lastTimeTag.Value);
            return collections;
        });

        static IEnumerable<KeyValuePair<double, TextIndex>> getInterpolatedTimeTagBetweenTwoTimeTag(KeyValuePair<double, TextIndex> firstTimeTag, KeyValuePair<double, TextIndex> secondTimeTag)
        {
            // we should not add the interpolation if timing is too small between two time-tags.
            var firstTimeTagTime = firstTimeTag.Key;
            var secondTimeTagTime = secondTimeTag.Key;
            if (Math.Abs(firstTimeTagTime - secondTimeTagTime) <= INTERPOLATION_TIMING * 2)
                yield break;

            // there's no need to add the interpolation if index are the same.
            if (firstTimeTag.Value.Index == secondTimeTag.Value.Index)
                yield break;

            var firstTimeTagIndex = firstTimeTag.Value;
            var secondTimeTagIndex = secondTimeTag.Value;
            var isLarger = firstTimeTag.Value < secondTimeTag.Value;

            var firstInterpolatedTimeTagIndex = isLarger ? TextIndexUtils.GetNextIndex(firstTimeTagIndex) : TextIndexUtils.GetPreviousIndex(firstTimeTagIndex);
            if (firstInterpolatedTimeTagIndex.Index != firstTimeTagIndex.Index)
                yield return new KeyValuePair<double, TextIndex>(firstTimeTagTime + INTERPOLATION_TIMING, firstInterpolatedTimeTagIndex);

            var secondInterpolatedTimeTag = isLarger ? TextIndexUtils.GetPreviousIndex(secondTimeTagIndex) : TextIndexUtils.GetNextIndex(secondTimeTagIndex);
            if (secondInterpolatedTimeTag.Index != secondTimeTagIndex.Index)
                yield return new KeyValuePair<double, TextIndex>(secondTimeTagTime - INTERPOLATION_TIMING, secondInterpolatedTimeTag);
        }
    }

    private float getTextIndexPosition(TextIndex index)
    {
        var leftTextIndexPosition = leftLyricText.GetTextIndexXPosition(index);
        var rightTextIndexPosition = rightLyricText.GetTextIndexXPosition(index);
        return index.State == TextIndex.IndexState.Start
            ? Math.Min(leftTextIndexPosition, rightTextIndexPosition)
            : Math.Max(leftTextIndexPosition, rightTextIndexPosition);
    }
}
