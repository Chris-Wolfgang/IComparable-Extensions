using System;

namespace Wolfgang.Extensions.IComparable.Tests.Unit;

public class ClampTests
{
    [Theory]
    [InlineData(15, 1, 10, 10)]
    [InlineData(0, 1, 10, 1)]
    [InlineData(5, 1, 10, 5)]
    [InlineData(1, 1, 10, 1)]
    [InlineData(10, 1, 10, 10)]
    [InlineData(7, 7, 7, 7)]
    [InlineData(3, 7, 7, 7)]
    [InlineData(-20, -10, -5, -10)]
    [InlineData(int.MaxValue, int.MinValue, 0, 0)]
    public void Clamp_when_called_on_integers_returns_expected_result(int value, int min, int max, int expected)
    {
        Assert.Equal(expected, value.Clamp(min, max));
    }



    [Theory]
    [InlineData(2026, 3, 31, 2026, 4, 1)]
    [InlineData(2026, 5, 15, 2026, 5, 15)]
    [InlineData(2026, 7, 4, 2026, 6, 30)]
    public void Clamp_when_called_on_date_times_returns_expected_result(int year, int month, int day, int expectedYear, int expectedMonth, int expectedDay)
    {
        var start = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2026, 6, 30, 0, 0, 0, DateTimeKind.Utc);

        Assert.Equal
        (
            new DateTime(expectedYear, expectedMonth, expectedDay, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc).Clamp(start, end)
        );
    }



    [Theory]
    [InlineData('a', 'c', 'x', 'c')]
    [InlineData('m', 'c', 'x', 'm')]
    [InlineData('z', 'c', 'x', 'x')]
    public void Clamp_when_called_on_chars_returns_expected_result(char value, char min, char max, char expected)
    {
        Assert.Equal(expected, value.Clamp(min, max));
    }



    [Fact]
    public void Clamp_when_called_on_strings_agrees_with_string_CompareTo()
    {
        var expected = "zebra".CompareTo("monkey") > 0 ? "monkey" : "zebra";

        Assert.Equal(expected, "zebra".Clamp("apple", "monkey"));
    }



    [Fact]
    public void Clamp_when_value_equals_a_bound_returns_the_value_instance()
    {
        var value = new string('b', 1);

        Assert.Same(value, value.Clamp("b", "c"));
    }



    [Fact]
    public void Clamp_when_min_is_null_defers_to_CompareTo()
    {
        // string.CompareTo(null) returns +1, so no string is below a null minimum.
        Assert.Equal("m", "m".Clamp(null!, "z"));
    }



    [Fact]
    public void Clamp_when_max_is_null_and_min_is_not_throws_ArgumentException()
    {
        // A null bound sorts first, so a non-null min is greater than a null max.
        var exception = Assert.Throws<ArgumentException>(() => "m".Clamp("a", null!));

        Assert.Equal("min", exception.ParamName);
    }



    [Fact]
    public void Clamp_when_bounds_are_inverted_throws_ArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => 5.Clamp(10, 1));

        Assert.Equal("min", exception.ParamName);
    }



    [Fact]
    public void Clamp_when_value_is_null_throws_ArgumentNullException()
    {
        string? value = null;

        var exception = Assert.Throws<ArgumentNullException>(() => value!.Clamp("a", "z"));

        Assert.Equal("value", exception.ParamName);
    }
}
