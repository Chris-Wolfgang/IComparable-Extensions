using System;

namespace Wolfgang.Extensions.IComparable;

/// <summary>
/// A collection of extension methods for types that implement IComparable
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IComparableExtensions
{
    /// <summary>
    /// Determines if the specified value is greater than the lowerBound and less than the upperBound
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="lowerBound">The lower end of the series to search.</param>
    /// <param name="upperBound">The upper end of the series to search.</param>
    /// <returns>
    /// True if the value to compare is greater than or equal to the min value and less
    /// than or equal to the max value. Otherwise, false.
    /// </returns>
    public static bool IsBetween<T>
    (
        this T value,
        T lowerBound,
        T upperBound
    ) where T : IComparable<T>
        => 0 < value.CompareTo(lowerBound) && value.CompareTo(upperBound) < 0;



    /// <summary>
    /// Determines if the specified value is greater than or equal to the
    /// lowerBound and less than or equal to the upperBound
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="lowerBound">The lower end of the series to search.</param>
    /// <param name="upperBound">The upper end of the series to search.</param>
    /// <returns>
    /// True if the value to compare is greater than or equal to the min value
    /// and less than or equal to the max value. Otherwise, false.
    /// </returns>
    public static bool IsInRange<T>
    (
        this T value,
        T lowerBound,
        T upperBound
    ) where T : IComparable<T>
        => 0 <= value.CompareTo(lowerBound) && value.CompareTo(upperBound) <= 0;
}
