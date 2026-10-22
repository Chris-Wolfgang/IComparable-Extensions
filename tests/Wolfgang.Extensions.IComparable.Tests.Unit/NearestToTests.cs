using System;
using System.Collections.Generic;
using System.Linq;

namespace Wolfgang.Extensions.IComparable.Tests.Unit;

public class NearestToTests
{
    private static readonly Func<int, int, int> IntDistance = (a, b) => Math.Abs(a - b);



    [Theory]
    [InlineData(5, 3)]
    [InlineData(0, 1)]
    [InlineData(100, 12)]
    [InlineData(10, 8)]
    [InlineData(8, 8)]
    public void NearestTo_when_called_on_integers_returns_the_closest_candidate(int value, int expected)
    {
        Assert.Equal(expected, value.NearestTo(IntDistance, 1, 3, 8, 12));
    }



    [Fact]
    public void NearestTo_when_two_candidates_are_equidistant_returns_the_first_one()
    {
        Assert.Equal(4, 5.NearestTo(IntDistance, 4, 6));
    }



    [Fact]
    public void NearestTo_when_two_candidates_are_equidistant_in_the_other_order_returns_the_first_one()
    {
        Assert.Equal(6, 5.NearestTo(IntDistance, 6, 4));
    }



    [Fact]
    public void NearestTo_when_there_is_a_single_candidate_returns_it()
    {
        Assert.Equal(42, 5.NearestTo(new[] { 42 }, IntDistance));
    }



    [Fact]
    public void NearestTo_when_called_on_date_times_returns_the_closest_milestone()
    {
        var milestones = new List<DateTime>
        {
            new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 3, 1, 0, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 6, 30, 0, 0, 0, DateTimeKind.Utc),
        };

        var nearest = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc)
            .NearestTo(milestones, (a, b) => (a - b).Duration());

        Assert.Equal(milestones[1], nearest);
    }



    [Fact]
    public void NearestTo_when_distance_is_a_double_uses_it()
    {
        Assert.Equal(2.5, 2.4.NearestTo(new[] { 1.0, 2.5, 4.0 }, (a, b) => Math.Abs(a - b)));
    }



    [Fact]
    public void NearestTo_passes_value_as_the_first_distance_argument()
    {
        var calls = new List<string>();
        5.NearestTo
        (
            new[] { 1, 9 },
            (a, b) =>
            {
                calls.Add($"{a},{b}");
                return Math.Abs(a - b);
            }
        );

        Assert.Equal(new[] { "5,1", "5,9" }, calls);
    }



    [Fact]
    public void NearestTo_when_candidates_contain_null_skips_them()
    {
        Assert.Equal("bb", "b".NearestTo(new[] { null!, "bb", null! }, (a, b) => Math.Abs(a.Length - b.Length)));
    }



    [Fact]
    public void NearestTo_when_candidates_are_empty_throws_ArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => 5.NearestTo(Enumerable.Empty<int>(), IntDistance));

        Assert.Equal("candidates", exception.ParamName);
    }



    [Fact]
    public void NearestTo_when_every_candidate_is_null_throws_ArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => "a".NearestTo(new string[] { null!, null! }, (_, _) => 0));

        Assert.Equal("candidates", exception.ParamName);
    }



    [Fact]
    public void NearestTo_when_the_params_array_is_empty_throws_ArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => 5.NearestTo(IntDistance));

        Assert.Equal("candidates", exception.ParamName);
    }



    [Fact]
    public void NearestTo_when_candidates_is_null_throws_ArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => 5.NearestTo((IEnumerable<int>)null!, IntDistance));

        Assert.Equal("candidates", exception.ParamName);
    }



    [Fact]
    public void NearestTo_when_distance_is_null_throws_ArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => 5.NearestTo(new[] { 1 }, (Func<int, int, int>)null!));

        Assert.Equal("distance", exception.ParamName);
    }



    [Fact]
    public void NearestTo_when_value_is_null_throws_ArgumentNullException()
    {
        string? value = null;

        var exception = Assert.Throws<ArgumentNullException>(() => value!.NearestTo(new[] { "a" }, (_, _) => 0));

        Assert.Equal("value", exception.ParamName);
    }
}
