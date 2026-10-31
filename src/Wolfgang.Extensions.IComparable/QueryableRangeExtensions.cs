using System;
using System.Linq;
using System.Linq.Expressions;

namespace Wolfgang.Extensions.IComparable;

/// <summary>
/// Range filters for <see cref="IQueryable{T}"/> that a LINQ provider (Entity Framework Core,
/// Entity Framework 6, ...) can translate to a server-side comparison, unlike
/// <see cref="IComparableExtensions.IsBetween{T}"/> and
/// <see cref="IComparableExtensions.IsInRange{T}"/>, which are opaque method calls to a provider
/// and force client-side evaluation.
/// </summary>
/// <remarks>
/// The filter is built as an expression tree. When <c>TKey</c> defines the comparison operators
/// (numeric types, <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="TimeSpan"/>,
/// <see cref="decimal"/>, ...) it uses them, which providers translate to <c>&gt;</c>,
/// <c>&lt;</c>, <c>&gt;=</c> and <c>&lt;=</c>. Otherwise (for example <see cref="string"/>) it
/// uses <c>key.CompareTo(bound) &gt; 0</c> and the like; for <see cref="string"/> that is
/// <see cref="string.CompareTo(string)"/>, which EF Core translates to a SQL comparison. Other
/// types without operators compare through <see cref="IComparable{T}.CompareTo"/>, which a
/// provider may not translate. The bounds are captured as closure values, so providers send
/// them as query parameters.
/// </remarks>
public static class QueryableRangeExtensions
{
    /// <summary>
    /// Filters <paramref name="source"/> to the elements whose key is strictly between
    /// <paramref name="lowerBound"/> and <paramref name="upperBound"/>: the server-side
    /// equivalent of <see cref="IComparableExtensions.IsBetween{T}"/>.
    /// </summary>
    /// <typeparam name="TSource">The element type.</typeparam>
    /// <typeparam name="TKey">The type of the key being compared.</typeparam>
    /// <param name="source">The query to filter.</param>
    /// <param name="keySelector">Selects the key to compare, for example <c>p =&gt; p.Price</c>.</param>
    /// <param name="lowerBound">The exclusive lower bound.</param>
    /// <param name="upperBound">The exclusive upper bound.</param>
    /// <returns>A query that keeps the elements with <c>lowerBound &lt; key &lt; upperBound</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// using System.Linq;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// var prices = new[] { 5m, 10m, 50m, 100m, 150m }.AsQueryable();
    ///
    /// // With EF Core this becomes: WHERE Price &gt; @lower AND Price &lt; @upper
    /// var midRange = prices.WhereBetween(p =&gt; p, 10m, 100m);   // 50
    /// </code>
    /// </example>
    public static IQueryable<TSource> WhereBetween<TSource, TKey>
    (
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector,
        TKey lowerBound,
        TKey upperBound
    ) where TKey : IComparable<TKey>
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (keySelector is null)
        {
            throw new ArgumentNullException(nameof(keySelector));
        }

        return source.Where
        (
            BuildRangePredicate(keySelector, lowerBound, upperBound, inclusive: false)
        );
    }



    /// <summary>
    /// Filters <paramref name="source"/> to the elements whose key is within
    /// <paramref name="lowerBound"/> and <paramref name="upperBound"/>, both inclusive: the
    /// server-side equivalent of <see cref="IComparableExtensions.IsInRange{T}"/>.
    /// </summary>
    /// <typeparam name="TSource">The element type.</typeparam>
    /// <typeparam name="TKey">The type of the key being compared.</typeparam>
    /// <param name="source">The query to filter.</param>
    /// <param name="keySelector">Selects the key to compare, for example <c>o =&gt; o.OrderDate</c>.</param>
    /// <param name="lowerBound">The inclusive lower bound.</param>
    /// <param name="upperBound">The inclusive upper bound.</param>
    /// <returns>A query that keeps the elements with <c>lowerBound &lt;= key &lt;= upperBound</c>.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="source"/> or <paramref name="keySelector"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// using System;
    /// using System.Linq;
    /// using Wolfgang.Extensions.IComparable;
    ///
    /// var orderDates = new[] { new DateTime(2026, 3, 31), new DateTime(2026, 4, 1), new DateTime(2026, 6, 30) }.AsQueryable();
    ///
    /// // With EF Core this becomes: WHERE OrderDate &gt;= @start AND OrderDate &lt;= @end
    /// var q2 = orderDates.WhereInRange(d =&gt; d, new DateTime(2026, 4, 1), new DateTime(2026, 6, 30));   // Apr 1, Jun 30
    /// </code>
    /// </example>
    public static IQueryable<TSource> WhereInRange<TSource, TKey>
    (
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector,
        TKey lowerBound,
        TKey upperBound
    ) where TKey : IComparable<TKey>
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (keySelector is null)
        {
            throw new ArgumentNullException(nameof(keySelector));
        }

        return source.Where
        (
            BuildRangePredicate(keySelector, lowerBound, upperBound, inclusive: true)
        );
    }



    /// <summary>
    /// Builds <c>x =&gt; lower &lt; key(x) &amp;&amp; key(x) &lt; upper</c> (or the inclusive form)
    /// over the selector's own parameter, so the result is a plain predicate a provider can
    /// translate.
    /// </summary>
    private static Expression<Func<TSource, bool>> BuildRangePredicate<TSource, TKey>
    (
        Expression<Func<TSource, TKey>> keySelector,
        TKey lowerBound,
        TKey upperBound,
        bool inclusive
    ) where TKey : IComparable<TKey>
    {
        var key = keySelector.Body;
        var lower = Capture(lowerBound);
        var upper = Capture(upperBound);

        var aboveLower = Compare<TKey>(key, lower, inclusive ? ExpressionType.GreaterThanOrEqual : ExpressionType.GreaterThan);
        var belowUpper = Compare<TKey>(key, upper, inclusive ? ExpressionType.LessThanOrEqual : ExpressionType.LessThan);

        return Expression.Lambda<Func<TSource, bool>>
        (
            Expression.AndAlso(aboveLower, belowUpper),
            keySelector.Parameters
        );
    }



    // A member access on a closure, exactly what the compiler emits for a captured local, so
    // providers parameterize the bound instead of inlining it as a SQL literal.
    private static Expression Capture<TKey>(TKey value)
    {
        Expression<Func<TKey>> captured = () => value;
        return captured.Body;
    }



    private static readonly System.Reflection.MethodInfo StringCompareTo =
        typeof(string).GetMethod(nameof(string.CompareTo), new[] { typeof(string) })!;



    private static Expression Compare<TKey>(Expression key, Expression bound, ExpressionType comparison)
        where TKey : IComparable<TKey>
    {
        if (OperatorSupport<TKey>.HasComparisonOperators)
        {
            return Expression.MakeBinary(comparison, key, bound);
        }

        // key.CompareTo(bound) <op> 0, the same call IsBetween / IsInRange make in memory
        // (ADR-0005). For string it calls string.CompareTo(string) itself, the shape EF Core
        // rewrites to a plain SQL comparison; a call through the IComparable<string> interface
        // (behind a Convert) is not recognised and fails translation. Other types go through
        // IComparable<TKey>.
        var compareToCall = typeof(TKey) == typeof(string)
            ? Expression.Call(key, StringCompareTo, bound)
            : Expression.Call
            (
                Expression.Convert(key, typeof(IComparable<TKey>)),
                typeof(IComparable<TKey>).GetMethod(nameof(IComparable<TKey>.CompareTo))!,
                bound
            );

        return Expression.MakeBinary
        (
            comparison,
            compareToCall,
            Expression.Constant(0)
        );
    }



    // Whether expression trees can apply > / < / >= / <= to TKey directly (built-in numeric
    // types, or a type that defines the operators). Probed once per TKey.
    private static class OperatorSupport<TKey>
    {
        public static readonly bool HasComparisonOperators = Probe();



        private static bool Probe()
        {
            var operand = Expression.Default(typeof(TKey));
            try
            {
                _ = Expression.GreaterThan(operand, operand);
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }
}
