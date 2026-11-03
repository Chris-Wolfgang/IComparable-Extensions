namespace Wolfgang.Extensions.IComparable.Tests.Fuzz;

public class FuzzPropertyAttributeTests
{
    [Theory]
    [InlineData(null, FuzzPropertyAttribute.DefaultMaxTest)]
    [InlineData("", FuzzPropertyAttribute.DefaultMaxTest)]
    [InlineData("abc", FuzzPropertyAttribute.DefaultMaxTest)]
    [InlineData("0", FuzzPropertyAttribute.DefaultMaxTest)]
    [InlineData("-5", FuzzPropertyAttribute.DefaultMaxTest)]
    [InlineData("1", 1)]
    [InlineData("100000", 100_000)]
    public void ResolveMaxTest_when_value_is_given_returns_expected_case_count(string? value, int expected)
    {
        Assert.Equal(expected, FuzzPropertyAttribute.ResolveMaxTest(value));
    }



    [Fact]
    public void Constructor_when_called_takes_MaxTest_from_FSCHECK_MAX_TESTS()
    {
        Assert.Equal
        (
            FuzzPropertyAttribute.ResolveMaxTest(Environment.GetEnvironmentVariable("FSCHECK_MAX_TESTS")),
            new FuzzPropertyAttribute().MaxTest
        );
    }
}
