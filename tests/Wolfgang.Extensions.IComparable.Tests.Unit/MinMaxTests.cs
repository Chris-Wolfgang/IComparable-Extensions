using System;
using System.Collections.Generic;
using System.Linq;

namespace Wolfgang.Extensions.IComparable.Tests.Unit;

public class MinMaxTests
{
    [Theory]
    [InlineData(3, 5, 3)]
    [InlineData(5, 3, 3)]
    [InlineData(4, 4, 4)]
    [InlineData(int.MinValue, int.MaxValue, int.MinValue)]
    public void Min_when_called_with_two_integers_returns_the_smaller(int value, int other, int expected)
    {
        Assert.Equal(expected, value.Min(other));
    }



    [Theory]
    [InlineData(3, 5, 5)]
    [InlineData(5, 3, 5)]
    [InlineData(4, 4, 4)]
    [InlineData(int.MinValue, int.MaxValue, int.MaxValue)]
    public void Max_when_called_with_two_integers_returns_the_larger(int value, int other, int expected)
    {
        Assert.Equal(expected, value.Max(other));
    }



    [Fact]
    public void Min_when_called_on_date_times_returns_the_earlier()
    {
        var deadline = new DateTime(2026, 9, 30, 0, 0, 0, DateTimeKind.Utc);
        var cap = new DateTime(2026, 6, 30, 0, 0, 0, DateTimeKind.Utc);

        Assert.Equal(cap, deadline.Min(cap));
    }



    [Fact]
    public void Max_when_called_on_versions_returns_the_later()
    {
        Assert.Equal(new Version(2, 0), new Version(1, 4).Max(new Version(2, 0)));
    }



    [Fact]
    public void Min_when_called_on_strings_agrees_with_string_CompareTo()
    {
        var expected = "pear".CompareTo("apple") < 0 ? "pear" : "apple";

        Assert.Equal(expected, "pear".Min("apple"));
    }



    [Fact]
    public void Min_when_values_compare_equal_returns_the_receiver_instance()
    {
        var value = new string('a', 1);

        Assert.Same(value, value.Min("a"));
    }



    [Fact]
    public void Max_when_values_compare_equal_returns_the_receiver_instance()
    {
        var value = new string('a', 1);

        Assert.Same(value, value.Max("a"));
    }



    [Fact]
    public void Min_when_other_is_null_ignores_it()
    {
        Assert.Equal("m", "m".Min((string)null!));
    }



    [Fact]
    public void Max_when_other_is_null_ignores_it()
    {
        Assert.Equal("m", "m".Max((string)null!));
    }



    [Theory]
    [InlineData(7, 3, 9, 5, 3)]
    [InlineData(1, 3, 9, 5, 1)]
    [InlineData(7, 8, 9, 0, 0)]
    public void Min_when_called_with_several_values_returns_the_smallest(int value, int other, int third, int fourth, int expected)
    {
        Assert.Equal(expected, value.Min(other, third, fourth));
    }



    [Theory]
    [InlineData(7, 3, 9, 5, 9)]
    [InlineData(10, 3, 9, 5, 10)]
    [InlineData(1, 2, 3, 40, 40)]
    public void Max_when_called_with_several_values_returns_the_largest(int value, int other, int third, int fourth, int expected)
    {
        Assert.Equal(expected, value.Max(other, third, fourth));
    }



    [Fact]
    public void Min_when_called_with_several_strings_skips_null_elements()
    {
        Assert.Equal("b", "m".Min("x", null!, "b"));
    }



    [Fact]
    public void Max_when_called_with_several_strings_skips_null_elements()
    {
        Assert.Equal("x", "m".Max(null!, "x", "b"));
    }



    [Fact]
    public void Min_when_the_params_array_is_null_throws_ArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => 5.Min(3, null!));

        Assert.Equal("others", exception.ParamName);
    }



    [Fact]
    public void Max_when_the_params_array_is_null_throws_ArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => 5.Max(3, null!));

        Assert.Equal("others", exception.ParamName);
    }



    [Fact]
    public void Min_when_called_with_a_sequence_returns_the_smallest()
    {
        Assert.Equal(4, 10.Min(new List<int> { 12, 4, 30 }));
    }



    [Fact]
    public void Max_when_called_with_a_sequence_returns_the_largest()
    {
        Assert.Equal(30, 10.Max(new List<int> { 12, 4, 30 }));
    }



    [Fact]
    public void Min_when_the_receiver_is_smaller_than_the_sequence_returns_the_receiver()
    {
        Assert.Equal(1, 1.Min(Enumerable.Range(2, 5)));
    }



    [Fact]
    public void Max_when_the_sequence_has_null_elements_skips_them()
    {
        IEnumerable<string> others = new[] { null!, "c", null! };

        Assert.Equal("c", "a".Max(others));
    }



    [Fact]
    public void Min_when_the_sequence_is_empty_throws_ArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => 5.Min(Enumerable.Empty<int>()));

        Assert.Equal("others", exception.ParamName);
    }



    [Fact]
    public void Max_when_the_sequence_is_empty_throws_ArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => 5.Max(new List<int>()));

        Assert.Equal("others", exception.ParamName);
    }



    [Fact]
    public void Min_when_the_sequence_is_null_throws_ArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => 5.Min((IEnumerable<int>)null!));

        Assert.Equal("others", exception.ParamName);
    }



    [Fact]
    public void Max_when_the_sequence_is_null_throws_ArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => 5.Max((IEnumerable<int>)null!));

        Assert.Equal("others", exception.ParamName);
    }



    [Fact]
    public void Min_when_value_is_null_throws_ArgumentNullException()
    {
        string? value = null;

        var exception = Assert.Throws<ArgumentNullException>(() => value!.Min("a"));

        Assert.Equal("value", exception.ParamName);
    }



    [Fact]
    public void Max_when_value_is_null_throws_ArgumentNullException()
    {
        string? value = null;

        var exception = Assert.Throws<ArgumentNullException>(() => value!.Max("a"));

        Assert.Equal("value", exception.ParamName);
    }



    [Fact]
    public void Min_with_a_sequence_when_value_is_null_throws_ArgumentNullException()
    {
        string? value = null;

        var exception = Assert.Throws<ArgumentNullException>(() => value!.Min(new[] { "a" }.AsEnumerable()));

        Assert.Equal("value", exception.ParamName);
    }



    [Fact]
    public void Max_with_a_sequence_when_value_is_null_throws_ArgumentNullException()
    {
        string? value = null;

        var exception = Assert.Throws<ArgumentNullException>(() => value!.Max(new[] { "a" }.AsEnumerable()));

        Assert.Equal("value", exception.ParamName);
    }



    [Fact]
    public void LINQ_Min_on_a_string_still_binds_to_Enumerable_Min()
    {
        // Guard: a string is IEnumerable<char>. The overloads here need at least one
        // argument, so a parameterless "cab".Min() must keep resolving to LINQ's Min over
        // the characters rather than to this library.
        Assert.Equal('a', "cab".Min());
    }



    [Fact]
    public void LINQ_Max_on_a_string_still_binds_to_Enumerable_Max()
    {
        Assert.Equal('c', "cab".Max());
    }
}
