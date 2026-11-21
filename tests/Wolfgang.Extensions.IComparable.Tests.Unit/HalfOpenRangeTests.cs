using System;
using System.Globalization;

namespace Wolfgang.Extensions.IComparable.Tests.Unit;

// ReSharper disable once InconsistentNaming
public class HalfOpenRangeTests
{
    [Theory]
    [InlineData(0, 1, 10, false)]
    [InlineData(1, 1, 10, true)]
    [InlineData(5, 1, 10, true)]
    [InlineData(9, 1, 10, true)]
    [InlineData(10, 1, 10, false)]
    [InlineData(11, 1, 10, false)]
    [InlineData(int.MinValue, int.MinValue, int.MaxValue, true)]
    [InlineData(int.MaxValue, int.MinValue, int.MaxValue, false)]
    public void IsInRangeExcludingUpper_when_called_on_integers_returns_expected_result(int value, int lowerBound, int upperBound, bool expected)
    {
        Assert.Equal(expected, value.IsInRangeExcludingUpper(lowerBound, upperBound));
    }



    [Theory]
    [InlineData(0, 1, 10, false)]
    [InlineData(1, 1, 10, false)]
    [InlineData(2, 1, 10, true)]
    [InlineData(5, 1, 10, true)]
    [InlineData(10, 1, 10, true)]
    [InlineData(11, 1, 10, false)]
    [InlineData(int.MinValue, int.MinValue, int.MaxValue, false)]
    [InlineData(int.MaxValue, int.MinValue, int.MaxValue, true)]
    public void IsInRangeExcludingLower_when_called_on_integers_returns_expected_result(int value, int lowerBound, int upperBound, bool expected)
    {
        Assert.Equal(expected, value.IsInRangeExcludingLower(lowerBound, upperBound));
    }



    [Theory]
    [InlineData(5, 5, 5)]
    [InlineData(5, 10, 1)]
    public void Half_open_methods_when_range_is_empty_or_reversed_return_false(int value, int lowerBound, int upperBound)
    {
        Assert.Equal
        (
            new[] { false, false },
            new[] { value.IsInRangeExcludingUpper(lowerBound, upperBound), value.IsInRangeExcludingLower(lowerBound, upperBound) }
        );
    }



    [Theory]
    [InlineData("2026/02/28", false, false)]
    [InlineData("2026/03/01", true, false)]
    [InlineData("2026/03/15", true, true)]
    [InlineData("2026/04/01", false, true)]
    [InlineData("2026/04/02", false, false)]
    public void Half_open_methods_when_called_on_date_times_return_expected_results
    (
        string value,
        bool excludingUpper,
        bool excludingLower
    )
    {
        var date = ParseUtc(value);
        var startOfMonth = ParseUtc("2026/03/01");
        var startOfNextMonth = ParseUtc("2026/04/01");

        Assert.Equal
        (
            new[] { excludingUpper, excludingLower },
            new[] { date.IsInRangeExcludingUpper(startOfMonth, startOfNextMonth), date.IsInRangeExcludingLower(startOfMonth, startOfNextMonth) }
        );
    }



    [Theory]
    [InlineData("a", true, false)]
    [InlineData("m", true, true)]
    [InlineData("z", false, true)]
    public void Half_open_methods_when_called_on_strings_agree_with_string_CompareTo
    (
        string value,
        bool excludingUpper,
        bool excludingLower
    )
    {
        Assert.Equal
        (
            new[] { excludingUpper, excludingLower },
            new[] { value.IsInRangeExcludingUpper("a", "z"), value.IsInRangeExcludingLower("a", "z") }
        );
    }



    [Fact]
    public void IsInRangeExcludingUpper_when_value_is_null_throws_ArgumentNullException()
    {
        string? value = null;

        var exception = Assert.Throws<ArgumentNullException>(() => value!.IsInRangeExcludingUpper("a", "z"));

        Assert.Equal("value", exception.ParamName);
    }



    [Fact]
    public void IsInRangeExcludingLower_when_value_is_null_throws_ArgumentNullException()
    {
        string? value = null;

        var exception = Assert.Throws<ArgumentNullException>(() => value!.IsInRangeExcludingLower("a", "z"));

        Assert.Equal("value", exception.ParamName);
    }



    [Fact]
    public void Half_open_methods_when_a_bound_is_null_defer_to_CompareTo_contract()
    {
        // string.CompareTo(null) returns +1: every non-null string is above a null lower bound and
        // above a null upper bound.
        Assert.Equal
        (
            new[] { true, false },
            new[] { "a".IsInRangeExcludingLower(null!, "z"), "a".IsInRangeExcludingUpper("a", null!) }
        );
    }



    private static DateTime ParseUtc(string value) =>
        DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
}
