using System;
using System.Globalization;

namespace Wolfgang.Extensions.IComparable.Tests.Unit;

// ReSharper disable once InconsistentNaming
public class ComparisonTests
{
    [Theory]
    [InlineData(5, 4, true)]
    [InlineData(5, 5, false)]
    [InlineData(5, 6, false)]
    [InlineData(int.MaxValue, int.MinValue, true)]
    [InlineData(int.MinValue, int.MaxValue, false)]
    public void IsGreaterThan_when_called_on_integers_returns_expected_result(int value, int other, bool expected)
    {
        Assert.Equal(expected, value.IsGreaterThan(other));
    }



    [Theory]
    [InlineData(5, 4, true)]
    [InlineData(5, 5, true)]
    [InlineData(5, 6, false)]
    public void IsGreaterThanOrEqualTo_when_called_on_integers_returns_expected_result(int value, int other, bool expected)
    {
        Assert.Equal(expected, value.IsGreaterThanOrEqualTo(other));
    }



    [Theory]
    [InlineData(5, 4, false)]
    [InlineData(5, 5, false)]
    [InlineData(5, 6, true)]
    [InlineData(int.MinValue, int.MaxValue, true)]
    public void IsLessThan_when_called_on_integers_returns_expected_result(int value, int other, bool expected)
    {
        Assert.Equal(expected, value.IsLessThan(other));
    }



    [Theory]
    [InlineData(5, 4, false)]
    [InlineData(5, 5, true)]
    [InlineData(5, 6, true)]
    public void IsLessThanOrEqualTo_when_called_on_integers_returns_expected_result(int value, int other, bool expected)
    {
        Assert.Equal(expected, value.IsLessThanOrEqualTo(other));
    }



    [Theory]
    [InlineData("2026/03/01", "2026/03/15", false, false, true, true)]
    [InlineData("2026/03/15", "2026/03/15", false, true, false, true)]
    [InlineData("2026/03/16", "2026/03/15", true, true, false, false)]
    public void Comparison_methods_when_called_on_date_times_return_expected_results
    (
        string value,
        string other,
        bool greaterThan,
        bool greaterThanOrEqualTo,
        bool lessThan,
        bool lessThanOrEqualTo
    )
    {
        var left = DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        var right = DateTime.Parse(other, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

        Assert.Equal
        (
            new[] { greaterThan, greaterThanOrEqualTo, lessThan, lessThanOrEqualTo },
            new[] { left.IsGreaterThan(right), left.IsGreaterThanOrEqualTo(right), left.IsLessThan(right), left.IsLessThanOrEqualTo(right) }
        );
    }



    [Theory]
    [InlineData('b', 'a', true)]
    [InlineData('a', 'a', false)]
    [InlineData('a', 'b', false)]
    public void IsGreaterThan_when_called_on_chars_returns_expected_result(char value, char other, bool expected)
    {
        Assert.Equal(expected, value.IsGreaterThan(other));
    }



    [Fact]
    public void IsLessThan_when_called_on_strings_agrees_with_string_CompareTo()
    {
        Assert.Equal("apple".CompareTo("banana") < 0, "apple".IsLessThan("banana"));
    }



    [Fact]
    public void IsGreaterThan_when_other_is_null_defers_to_CompareTo_contract()
    {
        // string.CompareTo(null) returns +1, so any non-null string is greater than null.
        Assert.True("a".IsGreaterThan(null!));
    }



    [Fact]
    public void IsLessThanOrEqualTo_when_other_is_null_defers_to_CompareTo_contract()
    {
        Assert.False("a".IsLessThanOrEqualTo(null!));
    }



    [Fact]
    public void IsGreaterThan_when_value_is_null_throws_ArgumentNullException()
    {
        string? value = null;

        var exception = Assert.Throws<ArgumentNullException>(() => value!.IsGreaterThan("a"));

        Assert.Equal("value", exception.ParamName);
    }



    [Fact]
    public void IsGreaterThanOrEqualTo_when_value_is_null_throws_ArgumentNullException()
    {
        string? value = null;

        var exception = Assert.Throws<ArgumentNullException>(() => value!.IsGreaterThanOrEqualTo("a"));

        Assert.Equal("value", exception.ParamName);
    }



    [Fact]
    public void IsLessThan_when_value_is_null_throws_ArgumentNullException()
    {
        string? value = null;

        var exception = Assert.Throws<ArgumentNullException>(() => value!.IsLessThan("a"));

        Assert.Equal("value", exception.ParamName);
    }



    [Fact]
    public void IsLessThanOrEqualTo_when_value_is_null_throws_ArgumentNullException()
    {
        string? value = null;

        var exception = Assert.Throws<ArgumentNullException>(() => value!.IsLessThanOrEqualTo("a"));

        Assert.Equal("value", exception.ParamName);
    }
}
