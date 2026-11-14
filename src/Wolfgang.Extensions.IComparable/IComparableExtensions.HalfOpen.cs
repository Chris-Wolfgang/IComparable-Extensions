using System;

namespace Wolfgang.Extensions.IComparable;

// ReSharper disable once InconsistentNaming
public static partial class IComparableExtensions
{
    /// <summary>
    /// Determines whether <paramref name="value"/> lies in the half-open interval
    /// [<paramref name="lowerBound"/>, <paramref name="upperBound"/>): greater than or equal to
    /// <paramref name="lowerBound"/> and strictly less than <paramref name="upperBound"/>.
    /// </summary>
    /// <remarks>
    /// The interval that suits consecutive ranges such as months, pages or array slices, where the
    /// upper bound of one range is the lower bound of the next. The bound semantics are in the
    /// method name, as for <see cref="IsBetween{T}"/> and <see cref="IsInRange{T}"/> (ADR-0001).
    /// </remarks>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="lowerBound">The inclusive lower bound of the range.</param>
    /// <param name="upperBound">The exclusive upper bound of the range.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is greater than or equal to
    /// <paramref name="lowerBound"/> and less than <paramref name="upperBound"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// using System;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// DateTime startOfMonth     = new DateTime(2026, 3, 1);
    /// DateTime startOfNextMonth = new DateTime(2026, 4, 1);
    ///
    /// bool first = startOfMonth.IsInRangeExcludingUpper(startOfMonth, startOfNextMonth);      // true
    /// bool next  = startOfNextMonth.IsInRangeExcludingUpper(startOfMonth, startOfNextMonth);  // false
    /// </code>
    /// </example>
    public static bool IsInRangeExcludingUpper<T>
    (
        this T value,
        T lowerBound,
        T upperBound
    ) where T : IComparable<T>
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return 0 <= value.CompareTo(lowerBound) && value.CompareTo(upperBound) < 0;
    }



    /// <summary>
    /// Determines whether <paramref name="value"/> lies in the half-open interval
    /// (<paramref name="lowerBound"/>, <paramref name="upperBound"/>]: strictly greater than
    /// <paramref name="lowerBound"/> and less than or equal to <paramref name="upperBound"/>.
    /// </summary>
    /// <remarks>
    /// The bound semantics are in the method name, as for <see cref="IsBetween{T}"/> and
    /// <see cref="IsInRange{T}"/> (ADR-0001).
    /// </remarks>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="lowerBound">The exclusive lower bound of the range.</param>
    /// <param name="upperBound">The inclusive upper bound of the range.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is greater than
    /// <paramref name="lowerBound"/> and less than or equal to <paramref name="upperBound"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// // Tiered pricing: the second tier is (100, 500] units.
    /// bool inTier    = 500.IsInRangeExcludingLower(100, 500);   // true  — 500 is included
    /// bool belowTier = 100.IsInRangeExcludingLower(100, 500);   // false — 100 belongs to tier one
    /// </code>
    /// </example>
    public static bool IsInRangeExcludingLower<T>
    (
        this T value,
        T lowerBound,
        T upperBound
    ) where T : IComparable<T>
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return 0 < value.CompareTo(lowerBound) && value.CompareTo(upperBound) <= 0;
    }
}
