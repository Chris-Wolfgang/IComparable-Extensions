using System;

namespace Wolfgang.Extensions.IComparable;

// ReSharper disable once InconsistentNaming
public static partial class IComparableExtensions
{
    /// <summary>
    /// Determines whether <paramref name="value"/> is strictly greater than <paramref name="other"/>,
    /// according to <see cref="IComparable{T}.CompareTo(T)"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="other">The value to compare <paramref name="value"/> against. It is passed to
    /// <see cref="IComparable{T}.CompareTo(T)"/> unchanged, so a <see langword="null"/>
    /// <paramref name="other"/> follows that type's contract.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is strictly greater than
    /// <paramref name="other"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// using System;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// int total = 120;
    /// bool overMinimum = total.IsGreaterThan(100);   // true
    /// bool notOver     = 100.IsGreaterThan(100);     // false — equal is not greater
    /// </code>
    /// </example>
    public static bool IsGreaterThan<T>
    (
        this T value,
        T other
    ) where T : IComparable<T>
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return 0 < value.CompareTo(other);
    }



    /// <summary>
    /// Determines whether <paramref name="value"/> is strictly less than <paramref name="other"/>,
    /// according to <see cref="IComparable{T}.CompareTo(T)"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="other">The value to compare <paramref name="value"/> against. It is passed to
    /// <see cref="IComparable{T}.CompareTo(T)"/> unchanged, so a <see langword="null"/>
    /// <paramref name="other"/> follows that type's contract.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is strictly less than
    /// <paramref name="other"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// using System;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// DateTime due = new DateTime(2026, 3, 1);
    /// bool overdue = due.IsLessThan(new DateTime(2026, 3, 15));   // true
    /// </code>
    /// </example>
    public static bool IsLessThan<T>
    (
        this T value,
        T other
    ) where T : IComparable<T>
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return value.CompareTo(other) < 0;
    }



    /// <summary>
    /// Determines whether <paramref name="value"/> is greater than or equal to <paramref name="other"/>,
    /// according to <see cref="IComparable{T}.CompareTo(T)"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="other">The value to compare <paramref name="value"/> against. It is passed to
    /// <see cref="IComparable{T}.CompareTo(T)"/> unchanged, so a <see langword="null"/>
    /// <paramref name="other"/> follows that type's contract.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is greater than or equal to
    /// <paramref name="other"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// using System;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// Version installed = new Version(2, 1);
    /// bool supported = installed.IsGreaterThanOrEqualTo(new Version(2, 0));   // true
    /// </code>
    /// </example>
    public static bool IsGreaterThanOrEqualTo<T>
    (
        this T value,
        T other
    ) where T : IComparable<T>
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return 0 <= value.CompareTo(other);
    }



    /// <summary>
    /// Determines whether <paramref name="value"/> is less than or equal to <paramref name="other"/>,
    /// according to <see cref="IComparable{T}.CompareTo(T)"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="other">The value to compare <paramref name="value"/> against. It is passed to
    /// <see cref="IComparable{T}.CompareTo(T)"/> unchanged, so a <see langword="null"/>
    /// <paramref name="other"/> follows that type's contract.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is less than or equal to
    /// <paramref name="other"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// using System;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// string name = "apple";
    /// bool inFirstHalf = name.IsLessThanOrEqualTo("m");   // true (culture-sensitive, see string.CompareTo)
    /// </code>
    /// </example>
    public static bool IsLessThanOrEqualTo<T>
    (
        this T value,
        T other
    ) where T : IComparable<T>
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return value.CompareTo(other) <= 0;
    }
}
