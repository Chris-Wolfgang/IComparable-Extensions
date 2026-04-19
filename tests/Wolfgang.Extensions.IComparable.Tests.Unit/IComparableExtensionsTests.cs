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
            var testValue = DateTime.Parse(value, CultureInfo.InvariantCulture);
            Assert.Equal(expectedValue, testValue.IsBetween(new DateTime(2011, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2011, 12, 31, 23, 59, 59, DateTimeKind.Utc)));
        }



        [Theory]
        [InlineData("2018/11/2", false)]
        [InlineData("2018/11/4", true)]
        [InlineData("2018/11/6", false)]
        public void IsBetween_when_called_on_specific_dates_returns_expected_result(DateTime value, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.IsBetween(new DateTime(2018, 11, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2018, 11, 6, 0, 0, 0, DateTimeKind.Utc)));
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
            var testValue = DateTime.Parse(value, CultureInfo.InvariantCulture);
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
            Assert.Equal(expectedValue, value.IsInRange(new DateTime(2018, 11, 2, 0, 0, 0, DateTimeKind.Utc), new DateTime(2018, 11, 6, 0, 0, 0, DateTimeKind.Utc)));
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
    }
