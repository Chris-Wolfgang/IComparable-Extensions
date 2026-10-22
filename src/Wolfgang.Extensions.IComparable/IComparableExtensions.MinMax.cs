using System;
using System.Collections.Generic;

namespace Wolfgang.Extensions.IComparable;

// ReSharper disable once InconsistentNaming
public static partial class IComparableExtensions
{
    /// <summary>
    /// Returns the smaller of <paramref name="value"/> and <paramref name="other"/>, according to
    /// <see cref="IComparable{T}.CompareTo(T)"/>. When values compare equal, the earliest one (starting with <paramref name="value"/>) is returned.
    /// A <see langword="null"/> candidate is ignored, so the result is never <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The first value.</param>
    /// <param name="other">The second value.</param>
    /// <returns>
    /// <paramref name="value"/> or <paramref name="other"/>, whichever is smaller.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// using System;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// DateTime deadline = new DateTime(2026, 9, 30);
    /// DateTime capped = deadline.Min(new DateTime(2026, 6, 30));   // 2026-06-30
    /// </code>
    /// </example>
    public static T Min<T>
    (
        this T value,
        T other
    ) where T : IComparable<T>
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return other is not null && other.CompareTo(value) < 0
            ? other
            : value;
    }



    /// <summary>
    /// Returns the smallest of <paramref name="value"/>, <paramref name="other"/> and every element of
    /// <paramref name="others"/>, according to <see cref="IComparable{T}.CompareTo(T)"/>. When values compare equal, the earliest one (starting with <paramref name="value"/>) is returned.
    /// A <see langword="null"/> candidate is ignored, so the result is never <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The first value.</param>
    /// <param name="other">The second value.</param>
    /// <param name="others">Any further values.</param>
    /// <returns>
    /// The smallest of the values.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> or <paramref name="others"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// int lowest = 7.Min(3, 9, 5);   // 3
    /// </code>
    /// </example>
    public static T Min<T>
    (
        this T value,
        T other,
        params T[] others
    ) where T : IComparable<T>
    {
        var result = value.Min(other);

        if (others is null)
        {
            throw new ArgumentNullException(nameof(others));
        }

        foreach (var item in others)
        {
            result = result.Min(item);
        }

        return result;
    }



    /// <summary>
    /// Returns the smallest of <paramref name="value"/> and every element of <paramref name="others"/>,
    /// according to <see cref="IComparable{T}.CompareTo(T)"/>. When values compare equal, the earliest one (starting with <paramref name="value"/>) is returned.
    /// A <see langword="null"/> candidate is ignored, so the result is never <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The first value.</param>
    /// <param name="others">The other values. It must contain at least one element.</param>
    /// <returns>
    /// The smallest of the values.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> or <paramref name="others"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="others"/> contains no elements.
    /// </exception>
    /// <example>
    /// <code>
    /// using System.Collections.Generic;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// int[] candidates = { 12, 4, 30 };
    /// int lowest = 10.Min(candidates);   // 4
    /// </code>
    /// </example>
    public static T Min<T>
    (
        this T value,
        IEnumerable<T> others
    ) where T : IComparable<T>
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (others is null)
        {
            throw new ArgumentNullException(nameof(others));
        }

        using var enumerator = others.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            throw new ArgumentException
            (
                "The sequence contains no elements.",
                nameof(others)
            );
        }

        var result = value.Min(enumerator.Current);
        while (enumerator.MoveNext())
        {
            result = result.Min(enumerator.Current);
        }

        return result;
    }



    /// <summary>
    /// Returns the larger of <paramref name="value"/> and <paramref name="other"/>, according to
    /// <see cref="IComparable{T}.CompareTo(T)"/>. When values compare equal, the earliest one (starting with <paramref name="value"/>) is returned.
    /// A <see langword="null"/> candidate is ignored, so the result is never <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The first value.</param>
    /// <param name="other">The second value.</param>
    /// <returns>
    /// <paramref name="value"/> or <paramref name="other"/>, whichever is larger.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// using System;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// Version installed = new Version(1, 4);
    /// Version effective = installed.Max(new Version(2, 0));   // 2.0
    /// </code>
    /// </example>
    public static T Max<T>
    (
        this T value,
        T other
    ) where T : IComparable<T>
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return other is not null && other.CompareTo(value) > 0
            ? other
            : value;
    }



    /// <summary>
    /// Returns the largest of <paramref name="value"/>, <paramref name="other"/> and every element of
    /// <paramref name="others"/>, according to <see cref="IComparable{T}.CompareTo(T)"/>. When values compare equal, the earliest one (starting with <paramref name="value"/>) is returned.
    /// A <see langword="null"/> candidate is ignored, so the result is never <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The first value.</param>
    /// <param name="other">The second value.</param>
    /// <param name="others">Any further values.</param>
    /// <returns>
    /// The largest of the values.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> or <paramref name="others"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// int highest = 7.Max(3, 9, 5);   // 9
    /// </code>
    /// </example>
    public static T Max<T>
    (
        this T value,
        T other,
        params T[] others
    ) where T : IComparable<T>
    {
        var result = value.Max(other);

        if (others is null)
        {
            throw new ArgumentNullException(nameof(others));
        }

        foreach (var item in others)
        {
            result = result.Max(item);
        }

        return result;
    }



    /// <summary>
    /// Returns the largest of <paramref name="value"/> and every element of <paramref name="others"/>,
    /// according to <see cref="IComparable{T}.CompareTo(T)"/>. When values compare equal, the earliest one (starting with <paramref name="value"/>) is returned.
    /// A <see langword="null"/> candidate is ignored, so the result is never <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values being compared.</typeparam>
    /// <param name="value">The first value.</param>
    /// <param name="others">The other values. It must contain at least one element.</param>
    /// <returns>
    /// The largest of the values.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> or <paramref name="others"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="others"/> contains no elements.
    /// </exception>
    /// <example>
    /// <code>
    /// using System.Collections.Generic;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// List&lt;int&gt; candidates = new List&lt;int&gt; { 12, 4, 30 };
    /// int highest = 10.Max(candidates);   // 30
    /// </code>
    /// </example>
    public static T Max<T>
    (
        this T value,
        IEnumerable<T> others
    ) where T : IComparable<T>
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (others is null)
        {
            throw new ArgumentNullException(nameof(others));
        }

        using var enumerator = others.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            throw new ArgumentException
            (
                "The sequence contains no elements.",
                nameof(others)
            );
        }

        var result = value.Max(enumerator.Current);
        while (enumerator.MoveNext())
        {
            result = result.Max(enumerator.Current);
        }

        return result;
    }
}
