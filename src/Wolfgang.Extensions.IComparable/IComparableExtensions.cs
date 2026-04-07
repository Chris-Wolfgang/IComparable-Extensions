using System;

namespace Wolfgang.Extensions.IComparable;

/// <summary>
/// A collection of extension methods for types that implement <see cref="IComparable{T}"/>.
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IComparableExtensions
{
    /// <summary>
    /// Determines if the specified <paramref name="value"/> is strictly greater than the
    /// <paramref name="lowerBound"/> and strictly less than the <paramref name="upperBound"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="lowerBound">The exclusive lower bound of the range.</param>
    /// <param name="upperBound">The exclusive upper bound of the range.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is greater than
    /// <paramref name="lowerBound"/> and less than <paramref name="upperBound"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static bool IsBetween<T>
    (
        this T value,
        T lowerBound,
        T upperBound
    ) where T : IComparable<T>
    {
        if (value is null) throw new ArgumentNullException(nameof(value));

        return 0 < value.CompareTo(lowerBound) && value.CompareTo(upperBound) < 0;
    }



    /// <summary>
    /// Determines if the specified <paramref name="value"/> is greater than or equal to the
    /// <paramref name="lowerBound"/> and less than or equal to the <paramref name="upperBound"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="lowerBound">The inclusive lower bound of the range.</param>
    /// <param name="upperBound">The inclusive upper bound of the range.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is greater than or equal to
    /// <paramref name="lowerBound"/> and less than or equal to <paramref name="upperBound"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static bool IsInRange<T>
    (
        this T value,
        T lowerBound,
        T upperBound
    ) where T : IComparable<T>
    {
        if (value is null) throw new ArgumentNullException(nameof(value));

        return 0 <= value.CompareTo(lowerBound) && value.CompareTo(upperBound) <= 0;
    }
}
