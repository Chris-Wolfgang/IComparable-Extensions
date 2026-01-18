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
        public void IsBetween_When_Called_On_Date_Times_Returns_Expected_Result(string value, bool expectedValue)
        {
            var testValue = DateTime.Parse(value);
            Assert.Equal(expectedValue, testValue.IsBetween(new DateTime(2011, 1, 1), new DateTime(2011, 12, 31, 23, 59, 59)));
        }



        [Theory]
        [InlineData("2018/11/2", false)]
        [InlineData("2018/11/4", true)]
        [InlineData("2018/11/6", false)]
        public void IsBetween_When_Called_On_Specific_Dates_Returns_Correct_Result(DateTime value, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.IsBetween(new DateTime(2018, 11, 2), new DateTime(2018, 11, 6)));
        }



        [Theory]
        [InlineData("aa", false)]
        [InlineData("ab", false)]
        [InlineData("am", true)]
        [InlineData("ay", false)]
        [InlineData("az", false)]
        public void IsBetween_When_Called_On_Strings_Returns_Expected_Result(string value, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.IsBetween("ab", "ay"));
        }



        [Theory]
        [InlineData('a', false)]
        [InlineData('b', false)]
        [InlineData('m', true)]
        [InlineData('y', false)]
        [InlineData('z', false)]
        public void IsBetween_When_Called_On_Chars_Returns_Expected_Result(char value, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.IsBetween('b', 'y'));
        }



        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(5, true)]
        [InlineData(10, true)]
        [InlineData(11, false)]
        public void IsInRange_When_Called_On_Integers_Returns_Expected_Result(int value, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.IsInRange(1, 10));
        }



        [Theory]
        [InlineData("2010/12/31 23:59:59", false)]
        [InlineData("2011/1/1", true)]
        [InlineData("2011/7/1", true)]
        [InlineData("2011/12/31 23:59:50", true)]
        [InlineData("2012/1/1", false)]
        public void IsInRange_When_Called_On_Date_Times_Returns_Expected_Result(string value, bool expectedValue)
        {
            var testValue = DateTime.Parse(value);
            Assert.Equal(expectedValue, testValue.IsInRange(new DateTime(2011, 1, 1), new DateTime(2011, 12, 31, 23, 59, 59)));
        }



        [Theory]
        [InlineData("2018/11/2")]
        [InlineData("2018/11/4")]
        [InlineData("2018/11/6")]
        public void IsInRange_When_Called_On_Specific_Dates_Returns_Correct_Result(DateTime value)
        {
            Assert.True(value.IsInRange(new DateTime(2018, 11, 2), new DateTime(2018, 11, 6)));
        }



        [Theory]
        [InlineData("a", false)]
        [InlineData("b", true)]
        [InlineData("m", true)]
        [InlineData("y", true)]
        [InlineData("z", false)]
        public void IsInRange_When_Called_On_Strings_Returns_Expected_Result(string value, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.IsInRange("b", "y"));
        }



        [Theory]
        [InlineData('a', false)]
        [InlineData('b', true)]
        [InlineData('m', true)]
        [InlineData('y', true)]
        [InlineData('z', false)]
        public void IsInRange_When_Called_On_Chars_Returns_Expected_Result(char value, bool expectedValue)
        {
            Assert.Equal(expectedValue, value.IsInRange('b', 'y'));
        }
    }
