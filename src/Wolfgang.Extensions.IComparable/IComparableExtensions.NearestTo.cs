using System;
using System.Collections.Generic;

namespace Wolfgang.Extensions.IComparable;

// ReSharper disable once InconsistentNaming
public static partial class IComparableExtensions
{
    /// <summary>
    /// Returns the element of <paramref name="candidates"/> closest to <paramref name="value"/>,
    /// where closeness is measured by <paramref name="distance"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="IComparable{T}"/> orders values but has no notion of how far apart two values
    /// are, so the caller supplies the distance (for example
    /// <c>(a, b) =&gt; Math.Abs(a - b)</c> or <c>(a, b) =&gt; (a - b).Duration()</c>). When two
    /// candidates are equally close, the one that appears first in
    /// <paramref name="candidates"/> is returned. <see langword="null"/> candidates are ignored.
    /// </remarks>
    /// <typeparam name="T">The type of the value and the candidates.</typeparam>
    /// <typeparam name="TDistance">The type of the distance, for example <see cref="int"/>,
    /// <see cref="double"/> or <see cref="TimeSpan"/>.</typeparam>
    /// <param name="value">The value to find the nearest candidate to.</param>
    /// <param name="candidates">The values to choose from. At least one must be non-null.</param>
    /// <param name="distance">Returns the distance between <paramref name="value"/> (first argument)
    /// and a candidate (second argument). Smaller means closer.</param>
    /// <returns>The candidate with the smallest distance to <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/>, <paramref name="candidates"/> or <paramref name="distance"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="candidates"/> contains no non-null element.
    /// </exception>
    /// <example>
    /// <code>
    /// using System;
    /// using System.Collections.Generic;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// var milestones = new List&lt;DateTime&gt;
    /// {
    ///     new DateTime(2026, 1, 15),
    ///     new DateTime(2026, 3, 1),
    ///     new DateTime(2026, 6, 30),
    /// };
    ///
    /// DateTime target = new DateTime(2026, 3, 15);
    /// DateTime nearest = target.NearestTo(milestones, (a, b) =&gt; (a - b).Duration());   // 2026-03-01
    /// </code>
    /// </example>
    public static T NearestTo<T, TDistance>
    (
        this T value,
        IEnumerable<T> candidates,
        Func<T, T, TDistance> distance
    )
        where T : IComparable<T>
        where TDistance : IComparable<TDistance>
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (candidates is null)
        {
            throw new ArgumentNullException(nameof(candidates));
        }

        if (distance is null)
        {
            throw new ArgumentNullException(nameof(distance));
        }

        var found = false;
        var nearest = value;
        var nearestDistance = default(TDistance);

        foreach (var candidate in candidates)
        {
            if (candidate is null)
            {
                continue;
            }

            var candidateDistance = distance(value, candidate);
            if (!found || candidateDistance.CompareTo(nearestDistance!) < 0)
            {
                found = true;
                nearest = candidate;
                nearestDistance = candidateDistance;
            }
        }

        if (!found)
        {
            throw new ArgumentException
            (
                "The candidates contain no non-null element.",
                nameof(candidates)
            );
        }

        return nearest;
    }



    /// <summary>
    /// Returns the candidate closest to <paramref name="value"/>, where closeness is measured by
    /// <paramref name="distance"/>. A <see langword="params"/> convenience form of
    /// <see cref="NearestTo{T, TDistance}(T, IEnumerable{T}, Func{T, T, TDistance})"/>, with the
    /// same tie, <see langword="null"/> and exception rules.
    /// </summary>
    /// <typeparam name="T">The type of the value and the candidates.</typeparam>
    /// <typeparam name="TDistance">The type of the distance.</typeparam>
    /// <param name="value">The value to find the nearest candidate to.</param>
    /// <param name="distance">Returns the distance between <paramref name="value"/> (first argument)
    /// and a candidate (second argument). Smaller means closer.</param>
    /// <param name="candidates">The values to choose from. At least one must be non-null.</param>
    /// <returns>The candidate with the smallest distance to <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/>, <paramref name="distance"/> or <paramref name="candidates"/> is
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="candidates"/> contains no non-null element.
    /// </exception>
    /// <example>
    /// <code>
    /// using System;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// int snapped = 5.NearestTo((a, b) =&gt; Math.Abs(a - b), 1, 3, 8, 12);   // 3
    /// </code>
    /// </example>
    public static T NearestTo<T, TDistance>
    (
        this T value,
        Func<T, T, TDistance> distance,
        params T[] candidates
    )
        where T : IComparable<T>
        where TDistance : IComparable<TDistance>
    {
        return value.NearestTo((IEnumerable<T>)candidates, distance);
    }
}
