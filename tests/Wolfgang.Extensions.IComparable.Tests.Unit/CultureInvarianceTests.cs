// ReSharper disable StringCompareToIsCultureSpecific — these helpers intentionally exercise string.CompareTo's culture-specific behaviour per ADR-0005
using System;
using System.Globalization;
using System.Threading;

namespace Wolfgang.Extensions.IComparable.Tests.Unit;

/// <summary>
/// Verifies the library adds no culture-sensitivity of its own — every call to
/// <see cref="IComparableExtensions.IsBetween{T}"/> / <see cref="IComparableExtensions.IsInRange{T}"/>
/// agrees with what <c>T.CompareTo(T)</c> would say under the same culture. When
/// <c>T = string</c>, that means the extensions propagate whatever culture-sensitivity
/// <see cref="string.CompareTo(string)"/> has (per ADR-0005). When <c>T</c> is a
/// culture-invariant type (int, DateTime, etc.), the extensions are culture-invariant too.
///
/// Every test runs under one of the hostile-culture rows (Turkish dotted-I, German
/// decimal-comma, simplified Chinese collation, Arabic Hindi-Arabic digits, Japanese
/// full-width digits) as well as en-US and Invariant. The <see cref="CultureScope"/>
/// IDisposable swaps <see cref="Thread.CurrentThread"/>'s current + UI culture for the
/// duration of the test and restores the originals after — so a hostile culture never
/// leaks into a subsequent test.
/// </summary>
public class CultureInvarianceTests
{
    public static readonly object[][] Cultures =
    {
        new object[] { "" },       // Invariant
        new object[] { "en-US" },
        new object[] { "tr-TR" },  // dotted / dotless I
        new object[] { "de-DE" },  // ß, äöü, comma decimal
        new object[] { "zh-CN" },  // simplified Chinese collation
        new object[] { "ar-SA" },  // right-to-left, Hindi-Arabic digits
        new object[] { "ja-JP" },  // full-width digits
    };


    [Theory]
    [MemberData(nameof(Cultures))]
    public void IsBetween_on_int_is_culture_invariant(string cultureName)
    {
        using var _ = new CultureScope(cultureName);

        Assert.True(5.IsBetween(1, 10));
        Assert.False(1.IsBetween(1, 10));       // exclusive lower
        Assert.False(10.IsBetween(1, 10));      // exclusive upper
        Assert.False(0.IsBetween(1, 10));
        Assert.False(11.IsBetween(1, 10));
    }


    [Theory]
    [MemberData(nameof(Cultures))]
    public void IsInRange_on_int_is_culture_invariant(string cultureName)
    {
        using var _ = new CultureScope(cultureName);

        Assert.True(5.IsInRange(1, 10));
        Assert.True(1.IsInRange(1, 10));        // inclusive lower
        Assert.True(10.IsInRange(1, 10));       // inclusive upper
        Assert.False(0.IsInRange(1, 10));
        Assert.False(11.IsInRange(1, 10));
    }


    [Theory]
    [MemberData(nameof(Cultures))]
    public void IsInRange_on_DateTime_is_culture_invariant(string cultureName)
    {
        using var _ = new CultureScope(cultureName);

        var q2Start = new DateTime(2026, 4, 1);
        var q2End = new DateTime(2026, 6, 30);
        var midQ2 = new DateTime(2026, 5, 15);

        Assert.True(midQ2.IsInRange(q2Start, q2End));
        Assert.True(q2Start.IsInRange(q2Start, q2End));
        Assert.True(q2End.IsInRange(q2Start, q2End));
        Assert.False(new DateTime(2026, 3, 31).IsInRange(q2Start, q2End));
        Assert.False(new DateTime(2026, 7, 1).IsInRange(q2Start, q2End));
    }


    [Theory]
    [MemberData(nameof(Cultures))]
    public void IsInRange_on_string_agrees_with_string_CompareTo_under_culture(string cultureName)
    {
        using var _ = new CultureScope(cultureName);

        // Per ADR-0005: when T is culture-sensitive (T.CompareTo depends on
        // CurrentCulture), the extension propagates that sensitivity. The test
        // does not hard-code an expected answer per culture — it asserts the
        // extension agrees with T.CompareTo under the same culture. Any divergence
        // would mean the library is adding a culture layer on top of T, which is
        // exactly what the ADR forbids.
        AssertMatchesCompareTo("m", "a", "z");
        AssertMatchesCompareTo("a", "a", "z");   // at lower
        AssertMatchesCompareTo("z", "a", "z");   // at upper
        AssertMatchesCompareTo("A", "a", "z");   // mixed case
    }


    [Theory]
    [MemberData(nameof(Cultures))]
    public void IsBetween_on_string_agrees_with_string_CompareTo_under_culture(string cultureName)
    {
        using var _ = new CultureScope(cultureName);

        // Same contract as IsInRange above, but exclusive bounds.
        AssertMatchesCompareToStrict("m", "a", "z");
        AssertMatchesCompareToStrict("a", "a", "z");   // at lower — exclusive so must be false
        AssertMatchesCompareToStrict("z", "a", "z");   // at upper — exclusive so must be false
        AssertMatchesCompareToStrict("A", "a", "z");
    }


    [Theory]
    [InlineData("en-US")]
    [InlineData("tr-TR")]
    public void turkish_dotted_and_dotless_I_range_matches_string_CompareTo(string cultureName)
    {
        using var _ = new CultureScope(cultureName);

        // The classic Turkish trap: "İ" (U+0130 dotted capital I), "ı" (U+0131
        // dotless small i). Their ordering vs "H", "I", "J", "i" varies by
        // culture. The library must NOT normalise; it must reflect whatever
        // string.CompareTo says under the active culture — the same value the
        // consumer would get from writing the CompareTo chain themselves.
        AssertMatchesCompareTo("İ", "H", "J");   // dotted capital I between H..J
        AssertMatchesCompareTo("ı", "H", "J");   // dotless small i between H..J
        AssertMatchesCompareTo("I", "H", "J");
        AssertMatchesCompareTo("i", "H", "J");
    }


    private static void AssertMatchesCompareTo(string value, string lower, string upper)
    {
        var expected = value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0;
        Assert.Equal(expected, value.IsInRange(lower, upper));
    }


    private static void AssertMatchesCompareToStrict(string value, string lower, string upper)
    {
        var expected = value.CompareTo(lower) > 0 && value.CompareTo(upper) < 0;
        Assert.Equal(expected, value.IsBetween(lower, upper));
    }
}


/// <summary>
/// Swaps <see cref="Thread.CurrentThread"/>'s current + UI culture for the lifetime
/// of the instance; restores the originals on <see cref="Dispose"/>. Used per-test so
/// hostile-culture assertions cannot leak into subsequent tests running on the same
/// thread.
/// </summary>
internal sealed class CultureScope : IDisposable
{
    private readonly CultureInfo _originalCurrent;
    private readonly CultureInfo _originalUI;


    public CultureScope(string cultureName)
    {
        _originalCurrent = Thread.CurrentThread.CurrentCulture;
        _originalUI = Thread.CurrentThread.CurrentUICulture;

        var culture = string.IsNullOrEmpty(cultureName)
            ? CultureInfo.InvariantCulture
            : CultureInfo.GetCultureInfo(cultureName);

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }


    public void Dispose()
    {
        Thread.CurrentThread.CurrentCulture = _originalCurrent;
        Thread.CurrentThread.CurrentUICulture = _originalUI;
    }
}
