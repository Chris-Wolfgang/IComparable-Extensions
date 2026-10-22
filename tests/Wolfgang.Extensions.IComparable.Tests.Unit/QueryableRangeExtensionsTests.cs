using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Wolfgang.Extensions.IComparable.Tests.Unit;

public class QueryableRangeExtensionsTests
{
    private static readonly Product[] Products =
    {
        new Product("pencil", 5m),
        new Product("ruler", 10m),
        new Product("stapler", 50m),
        new Product("lamp", 100m),
        new Product("chair", 150m),
    };



    [Fact]
    public void WhereBetween_when_called_on_decimals_keeps_the_strictly_inside_elements()
    {
        var names = Products.AsQueryable().WhereBetween(p => p.Price, 10m, 100m).Select(p => p.Name);

        Assert.Equal(new[] { "stapler" }, names.ToArray());
    }



    [Fact]
    public void WhereInRange_when_called_on_decimals_keeps_the_bounds_too()
    {
        var names = Products.AsQueryable().WhereInRange(p => p.Price, 10m, 100m).Select(p => p.Name);

        Assert.Equal(new[] { "ruler", "stapler", "lamp" }, names.ToArray());
    }



    [Theory]
    [InlineData(1, 10, new[] { 2, 3, 4, 5, 6, 7, 8, 9 })]
    [InlineData(5, 5, new int[0])]
    [InlineData(10, 1, new int[0])]
    public void WhereBetween_when_called_on_integers_matches_IsBetween(int lower, int upper, int[] expected)
    {
        var source = Enumerable.Range(0, 12).ToArray();

        Assert.Equal(expected, source.AsQueryable().WhereBetween(x => x, lower, upper).ToArray());
    }



    [Fact]
    public void WhereInRange_when_called_on_date_times_keeps_the_boundary_days()
    {
        var start = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 6, 30, 0, 0, 0, DateTimeKind.Utc);
        var dates = new[] { start.AddDays(-1), start, start.AddDays(40), end, end.AddDays(1) };

        Assert.Equal(new[] { start, start.AddDays(40), end }, dates.AsQueryable().WhereInRange(d => d, start, end).ToArray());
    }



    [Fact]
    public void WhereBetween_when_called_on_strings_agrees_with_IsBetween()
    {
        var expected = Products.Where(p => p.Name.IsBetween("d", "r")).Select(p => p.Name).ToArray();

        var actual = Products.AsQueryable().WhereBetween(p => p.Name, "d", "r").Select(p => p.Name).ToArray();

        Assert.Equal(expected, actual);
    }



    [Fact]
    public void WhereInRange_when_called_on_strings_agrees_with_IsInRange()
    {
        var expected = Products.Where(p => p.Name.IsInRange("lamp", "ruler")).Select(p => p.Name).ToArray();

        var actual = Products.AsQueryable().WhereInRange(p => p.Name, "lamp", "ruler").Select(p => p.Name).ToArray();

        Assert.Equal(expected, actual);
    }



    [Fact]
    public void WhereBetween_on_a_type_with_operators_builds_a_plain_comparison()
    {
        var predicate = PredicateOf(Products.AsQueryable().WhereBetween(p => p.Price, 10m, 100m));

        Assert.Equal(ExpressionType.GreaterThan, ((BinaryExpression)((BinaryExpression)predicate.Body).Left).NodeType);
        Assert.Equal(ExpressionType.LessThan, ((BinaryExpression)((BinaryExpression)predicate.Body).Right).NodeType);
    }



    [Fact]
    public void WhereInRange_on_a_type_with_operators_builds_inclusive_comparisons()
    {
        var predicate = PredicateOf(Products.AsQueryable().WhereInRange(p => p.Price, 10m, 100m));

        Assert.Equal(ExpressionType.GreaterThanOrEqual, ((BinaryExpression)((BinaryExpression)predicate.Body).Left).NodeType);
        Assert.Equal(ExpressionType.LessThanOrEqual, ((BinaryExpression)((BinaryExpression)predicate.Body).Right).NodeType);
    }



    [Fact]
    public void WhereBetween_on_a_type_without_operators_compares_through_CompareTo()
    {
        var predicate = PredicateOf(Products.AsQueryable().WhereBetween(p => p.Name, "d", "r"));

        var lower = (BinaryExpression)((BinaryExpression)predicate.Body).Left;
        var call = (MethodCallExpression)lower.Left;

        Assert.Equal(nameof(IComparable<string>.CompareTo), call.Method.Name);
    }



    [Fact]
    public void WhereBetween_captures_the_bounds_as_closure_members_not_constants()
    {
        // A member access on a closure is what providers turn into a query parameter; a
        // ConstantExpression would be inlined into the SQL text.
        var predicate = PredicateOf(Products.AsQueryable().WhereBetween(p => p.Price, 10m, 100m));

        var lower = (BinaryExpression)((BinaryExpression)predicate.Body).Left;

        Assert.Equal(ExpressionType.MemberAccess, lower.Right.NodeType);
    }



    [Fact]
    public void WhereBetween_reuses_the_selector_parameter()
    {
        Expression<Func<Product, decimal>> selector = p => p.Price;

        var predicate = PredicateOf(Products.AsQueryable().WhereBetween(selector, 10m, 100m));

        Assert.Same(selector.Parameters[0], predicate.Parameters[0]);
    }



    [Fact]
    public void WhereBetween_when_source_is_null_throws_ArgumentNullException()
    {
        IQueryable<Product> source = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => source.WhereBetween(p => p.Price, 1m, 2m));

        Assert.Equal("source", exception.ParamName);
    }



    [Fact]
    public void WhereBetween_when_keySelector_is_null_throws_ArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Products.AsQueryable().WhereBetween((Expression<Func<Product, decimal>>)null!, 1m, 2m));

        Assert.Equal("keySelector", exception.ParamName);
    }



    [Fact]
    public void WhereInRange_when_source_is_null_throws_ArgumentNullException()
    {
        IQueryable<Product> source = null!;

        var exception = Assert.Throws<ArgumentNullException>(() => source.WhereInRange(p => p.Price, 1m, 2m));

        Assert.Equal("source", exception.ParamName);
    }



    [Fact]
    public void WhereInRange_when_keySelector_is_null_throws_ArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => Products.AsQueryable().WhereInRange((Expression<Func<Product, decimal>>)null!, 1m, 2m));

        Assert.Equal("keySelector", exception.ParamName);
    }



    private static LambdaExpression PredicateOf<TSource>(IQueryable<TSource> query)
    {
        var where = (MethodCallExpression)query.Expression;
        return (LambdaExpression)((UnaryExpression)where.Arguments[1]).Operand;
    }



    [ExcludeFromCodeCoverage]
    private sealed class Product
    {
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }



        public string Name { get; }



        public decimal Price { get; }
    }
}
