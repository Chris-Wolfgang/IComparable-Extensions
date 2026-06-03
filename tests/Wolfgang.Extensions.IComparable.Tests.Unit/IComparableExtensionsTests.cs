using System.Globalization;

namespace Wolfgang.Extensions.IComparable.Tests.Unit;

// ReSharper disable once InconsistentNaming
public class IComparableExtensionsTests
{

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(5, true)]
    [InlineData(10, false)]
    [InlineData(11, false)]
    public void IsBetween_when_called_on_integers_returns_expected_result(int value, bool expectedValue)
    {
        Assert.Equal(expectedValue, value.IsBetween(1, 10));
    }



    [Theory]
    [InlineData("2010/12/31 23:59:59", false)]
    [InlineData("2011/1/1", false)]
    [InlineData("2011/7/1", true)]
    [InlineData("2011/12/31 23:59:59", false)]
    [InlineData("2012/1/1", false)]
    public void IsBetween_when_called_on_date_times_returns_expected_result(string value, bool expectedValue)
    {
        var testValue = DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        Assert.Equal(expectedValue, testValue.IsBetween(new DateTime(2011, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2011, 12, 31, 23, 59, 59, DateTimeKind.Utc)));
    }



    [Theory]
    [InlineData("2018/11/2", false)]
    [InlineData("2018/11/4", true)]
    [InlineData("2018/11/6", false)]
    public void IsBetween_when_called_on_specific_dates_returns_expected_result(DateTime value, bool expectedValue)
    {
        var utcValue = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        Assert.Equal(expectedValue, utcValue.IsBetween(new DateTime(2018, 11, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2018, 11, 6, 0, 0, 0, DateTimeKind.Utc)));
    }



    [Theory]
    [InlineData("aa", false)]
    [InlineData("ab", false)]
    [InlineData("am", true)]
    [InlineData("ay", false)]
    [InlineData("az", false)]
    public void IsBetween_when_called_on_strings_returns_expected_result(string value, bool expectedValue)
    {
        Assert.Equal(expectedValue, value.IsBetween("ab", "ay"));
    }



    [Theory]
    [InlineData('a', false)]
    [InlineData('b', false)]
    [InlineData('m', true)]
    [InlineData('y', false)]
    [InlineData('z', false)]
    public void IsBetween_when_called_on_chars_returns_expected_result(char value, bool expectedValue)
    {
        Assert.Equal(expectedValue, value.IsBetween('b', 'y'));
    }



    [Fact]
    public void IsBetween_when_value_equals_both_bounds_returns_false()
    {
        Assert.False(5.IsBetween(5, 5));
    }



    [Fact]
    public void IsBetween_when_bounds_are_inverted_returns_false()
    {
        Assert.False(5.IsBetween(10, 1));
    }



    [Fact]
    public void IsBetween_when_value_is_null_throws_argument_null_exception()
    {
        string? value = null;

        Assert.Throws<ArgumentNullException>(() => value!.IsBetween("a", "z"));
    }



    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(5, true)]
    [InlineData(10, true)]
    [InlineData(11, false)]
    public void IsInRange_when_called_on_integers_returns_expected_result(int value, bool expectedValue)
    {
        Assert.Equal(expectedValue, value.IsInRange(1, 10));
    }



    [Theory]
    [InlineData("2010/12/31 23:59:59", false)]
    [InlineData("2011/1/1", true)]
    [InlineData("2011/7/1", true)]
    [InlineData("2011/12/31 23:59:50", true)]
    [InlineData("2012/1/1", false)]
    public void IsInRange_when_called_on_date_times_returns_expected_result(string value, bool expectedValue)
    {
        var testValue = DateTime.Parse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
        Assert.Equal(expectedValue, testValue.IsInRange(new DateTime(2011, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2011, 12, 31, 23, 59, 59, DateTimeKind.Utc)));
    }



    [Theory]
    [InlineData("2018/11/1", false)]
    [InlineData("2018/11/2", true)]
    [InlineData("2018/11/4", true)]
    [InlineData("2018/11/6", true)]
    [InlineData("2018/11/7", false)]
    public void IsInRange_when_called_on_specific_dates_returns_expected_result(DateTime value, bool expectedValue)
    {
        var utcValue = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        Assert.Equal(expectedValue, utcValue.IsInRange(new DateTime(2018, 11, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2018, 11, 6, 0, 0, 0, DateTimeKind.Utc)));
    }



    [Theory]
    [InlineData("a", false)]
    [InlineData("b", true)]
    [InlineData("m", true)]
    [InlineData("y", true)]
    [InlineData("z", false)]
    public void IsInRange_when_called_on_strings_returns_expected_result(string value, bool expectedValue)
    {
        Assert.Equal(expectedValue, value.IsInRange("b", "y"));
    }



    [Theory]
    [InlineData('a', false)]
    [InlineData('b', true)]
    [InlineData('m', true)]
    [InlineData('y', true)]
    [InlineData('z', false)]
    public void IsInRange_when_called_on_chars_returns_expected_result(char value, bool expectedValue)
    {
        Assert.Equal(expectedValue, value.IsInRange('b', 'y'));
    }



    [Fact]
    public void IsInRange_when_value_equals_both_bounds_returns_true()
    {
        Assert.True(5.IsInRange(5, 5));
    }



    [Fact]
    public void IsInRange_when_bounds_are_inverted_returns_false()
    {
        Assert.False(5.IsInRange(10, 1));
    }



    [Fact]
    public void IsInRange_when_value_is_null_throws_argument_null_exception()
    {
        string? value = null;

        Assert.Throws<ArgumentNullException>(() => value!.IsInRange("a", "z"));
    }



    // ---------------------------------------------------------------
    // T4 audit additions — exercise generic dispatch through a custom
    // IComparable<T> type, and lock in the *currently undocumented*
    // behaviour when bound arguments are null. The public XML docs
    // describe only the value-null contract; bound-null behaviour is
    // an implementation consequence of deferring to T.CompareTo, and
    // these tests pin it so a refactor can't silently change it.
    //
    // Concretely: the methods only null-check `value`; null bounds
    // are passed through to T.CompareTo. For string,
    // "m".CompareTo(null) returns +1, so "m".IsInRange(null, "z")
    // is true (the lower-bound check passes) and
    // "m".IsBetween("a", null) is false (the upper-bound check
    // requires "m".CompareTo(null) < 0, which fails).
    // ---------------------------------------------------------------

    private sealed class Money : IComparable<Money>
    {
        public decimal Amount { get; }

        public Money(decimal amount) => Amount = amount;

        public int CompareTo(Money? other) =>
            other is null ? 1 : Amount.CompareTo(other.Amount);
    }



    [Fact]
    public void IsBetween_when_called_on_custom_IComparable_T_returns_expected_result()
    {
        var ten     = new Money(10m);
        var twenty  = new Money(20m);
        var fifteen = new Money(15m);

        Assert.True(fifteen.IsBetween(ten, twenty));
        Assert.False(ten.IsBetween(ten, twenty));     // strict lower
        Assert.False(twenty.IsBetween(ten, twenty));  // strict upper
    }



    [Fact]
    public void IsInRange_when_called_on_custom_IComparable_T_returns_expected_result()
    {
        var ten     = new Money(10m);
        var twenty  = new Money(20m);
        var fifteen = new Money(15m);

        Assert.True(fifteen.IsInRange(ten, twenty));
        Assert.True(ten.IsInRange(ten, twenty));      // inclusive lower
        Assert.True(twenty.IsInRange(ten, twenty));   // inclusive upper
    }



    [Fact]
    public void IsBetween_when_lowerBound_is_null_defers_to_CompareTo_contract()
    {
        // string.CompareTo(null) returns +1, so 0 < +1 is true for
        // the lower-bound check, and "m".CompareTo("z") < 0 is true
        // for the upper-bound check — net result is true.
        Assert.True("m".IsBetween(null!, "z"));
    }



    [Fact]
    public void IsInRange_when_lowerBound_is_null_defers_to_CompareTo_contract()
    {
        Assert.True("m".IsInRange(null!, "z"));
    }



    [Fact]
    public void IsBetween_when_upperBound_is_null_defers_to_CompareTo_contract()
    {
        // string.CompareTo(null) returns +1. The upper-bound check
        // is value.CompareTo(upperBound) < 0 → "m".CompareTo(null) < 0
        // → 1 < 0 → false. Net result is false.
        Assert.False("m".IsBetween("a", null!));
    }



    [Fact]
    public void IsInRange_when_upperBound_is_null_defers_to_CompareTo_contract()
    {
        // Same reasoning: value.CompareTo(upperBound) <= 0 →
        // "m".CompareTo(null) <= 0 → 1 <= 0 → false.
        Assert.False("m".IsInRange("a", null!));
    }



    // ---------------------------------------------------------------
    // Edge-case integer coverage — negative bounds + value-type
    // extremes. The behaviour is the same as the positive cases by
    // construction, but these rows lock in that the implementation
    // doesn't accidentally specialise on positive numbers (e.g. via
    // an unsigned compare or an `Abs`-based shortcut introduced
    // during a future refactor).
    // ---------------------------------------------------------------

    [Theory]
    [InlineData(-11, false)]   // below the negative-to-zero range
    [InlineData(-10, false)]   // exact lower (strict)
    [InlineData(-5, true)]     // mid
    [InlineData(0, false)]     // exact upper (strict)
    [InlineData(1, false)]     // above
    public void IsBetween_when_called_on_negative_integer_range_returns_expected_result(int value, bool expectedValue)
    {
        Assert.Equal(expectedValue, value.IsBetween(-10, 0));
    }



    [Theory]
    [InlineData(-11, false)]   // below
    [InlineData(-10, true)]    // exact lower (inclusive)
    [InlineData(-5, true)]     // mid
    [InlineData(0, true)]      // exact upper (inclusive)
    [InlineData(1, false)]     // above
    public void IsInRange_when_called_on_negative_integer_range_returns_expected_result(int value, bool expectedValue)
    {
        Assert.Equal(expectedValue, value.IsInRange(-10, 0));
    }



    [Fact]
    public void IsBetween_when_value_equals_int_MaxValue_against_full_range_returns_false()
    {
        // strict upper — MaxValue is NOT > MaxValue
        Assert.False(int.MaxValue.IsBetween(int.MinValue, int.MaxValue));
    }



    [Fact]
    public void IsBetween_when_value_equals_int_MinValue_against_full_range_returns_false()
    {
        // strict lower — MinValue is NOT < MinValue
        Assert.False(int.MinValue.IsBetween(int.MinValue, int.MaxValue));
    }



    [Fact]
    public void IsInRange_when_value_equals_int_MaxValue_against_full_range_returns_true()
    {
        Assert.True(int.MaxValue.IsInRange(int.MinValue, int.MaxValue));
    }



    [Fact]
    public void IsInRange_when_value_equals_int_MinValue_against_full_range_returns_true()
    {
        Assert.True(int.MinValue.IsInRange(int.MinValue, int.MaxValue));
    }
}
