using System;
using System.Collections.Generic;

namespace Wolfgang.Extensions.IComparable;

// ReSharper disable once InconsistentNaming
public static partial class IComparableExtensions
{
    /// <summary>
    /// Returns <paramref name="value"/> constrained to the inclusive range
    /// [<paramref name="min"/>, <paramref name="max"/>]: <paramref name="min"/> when
    /// <paramref name="value"/> is less than it, <paramref name="max"/> when
    /// <paramref name="value"/> is greater than it, otherwise <paramref name="value"/> itself.
    /// Works for any <see cref="IComparable{T}"/>, including types <c>Math.Clamp</c> does not
    /// cover (<see cref="DateTime"/>, <see cref="string"/>, <see cref="Version"/>, custom types)
    /// and on frameworks that predate <c>Math.Clamp</c>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value to clamp.</param>
    /// <param name="min">The inclusive lower bound.</param>
    /// <param name="max">The inclusive upper bound.</param>
    /// <returns>
    /// <paramref name="min"/>, <paramref name="max"/> or <paramref name="value"/>, as described
    /// above. When <paramref name="value"/> compares equal to a bound, <paramref name="value"/>
    /// itself is returned.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="min"/> is greater than <paramref name="max"/> (the same rule as
    /// <c>Math.Clamp</c>), as decided by <see cref="Comparer{T}.Default"/>, which orders a
    /// <see langword="null"/> bound before every other value.
    /// </exception>
    /// <example>
    /// <code>
    /// using System;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// int high = 15.Clamp(1, 10);   // 10
    /// int low  = 0.Clamp(1, 10);    // 1
    /// int mid  = 5.Clamp(1, 10);    // 5
    ///
    /// DateTime requested = new DateTime(2026, 7, 4);
    /// DateTime booked = requested.Clamp(new DateTime(2026, 4, 1), new DateTime(2026, 6, 30));   // 2026-06-30
    /// </code>
    /// </example>
    public static T Clamp<T>
    (
        this T value,
        T min,
        T max
    ) where T : IComparable<T>
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        // Comparer<T>.Default orders a null bound before every non-null value, so a null
        // min never trips the check and the bounds keep their CompareTo semantics below.
        if (Comparer<T>.Default.Compare(min, max) > 0)
        {
            throw new ArgumentException
            (
                "The minimum must be less than or equal to the maximum.",
                nameof(min)
            );
        }

        if (value.CompareTo(min) < 0)
        {
            return min;
        }

        return value.CompareTo(max) > 0
            ? max
            : value;
    }
}
