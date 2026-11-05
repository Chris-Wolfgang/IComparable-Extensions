// ReSharper disable StringCompareToIsCultureSpecific — string properties intentionally exercise culture-specific CompareTo per ADR-0005
using System.Diagnostics.CodeAnalysis;
using FsCheck;
using FsCheck.Fluent;
using FsCheck.Xunit;

namespace Wolfgang.Extensions.IComparable.Tests.Fuzz;

/// <summary>
/// Property-based fuzz tests for <see cref="IComparableExtensions"/>. The core
/// property under test is: the extension methods must agree with a hand-written
/// <c>T.CompareTo</c> chain for every input the property engine generates. Any
/// disagreement means the library added or lost a semantic on top of what a
/// consumer would get from writing the same comparison themselves — the exact
/// class of drift a unit-test suite targeted at known cases will not catch.
///
/// The library also propagates <c>T.CompareTo</c>'s null-bound behaviour
/// (per ADR-0003), so bounds are allowed to be null; only the receiver
/// (<c>value</c>) is null-checked, and the properties assert that a null
/// receiver throws <see cref="ArgumentNullException"/>.
///
/// Each property runs <see cref="FuzzPropertyAttribute.DefaultMaxTest"/> (100) cases so
/// <c>dotnet test</c> stays fast for PR CI. <see cref="FuzzPropertyAttribute"/> reads the
/// <c>FSCHECK_MAX_TESTS</c> environment variable, which the weekly <c>fuzz.yaml</c>
/// workflow sets to 100,000 for the "CI-time hours" run (#157).
/// </summary>
[SuppressMessage("Major Code Smell", "S101:Class names should comply with a naming convention", Justification = "Named to mirror the IComparableExtensions class whose properties it fuzzes.")]
public class IComparableExtensionsProperties
{
    // -- int -----------------------------------------------------------------

    [FuzzProperty]
    public bool IsBetween_on_int_matches_manual_CompareTo(int value, int lower, int upper) =>
        value.IsBetween(lower, upper)
            == (value.CompareTo(lower) > 0 && value.CompareTo(upper) < 0);



    [FuzzProperty]
    public bool IsInRange_on_int_matches_manual_CompareTo(int value, int lower, int upper) =>
        value.IsInRange(lower, upper)
            == (value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0);



    // -- long ----------------------------------------------------------------

    [FuzzProperty]
    public bool IsBetween_on_long_matches_manual_CompareTo(long value, long lower, long upper) =>
        value.IsBetween(lower, upper)
            == (value.CompareTo(lower) > 0 && value.CompareTo(upper) < 0);



    [FuzzProperty]
    public bool IsInRange_on_long_matches_manual_CompareTo(long value, long lower, long upper) =>
        value.IsInRange(lower, upper)
            == (value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0);



    // -- double --------------------------------------------------------------
    // NaN is intentionally excluded — string.CompareTo and double.CompareTo
    // disagree on NaN behaviour by design (double treats NaN as equal to NaN
    // for sorting), and the library's contract is "whatever T.CompareTo does".
    // The properties are trivially true when NaN appears; we filter it upstream
    // rather than let it produce noise in property counter-examples.

    [FuzzProperty]
    public Property IsBetween_on_double_matches_manual_CompareTo() =>
        Prop.ForAll<double, double, double>((value, lower, upper) =>
            double.IsNaN(value) || double.IsNaN(lower) || double.IsNaN(upper)
                || value.IsBetween(lower, upper)
                    == (value.CompareTo(lower) > 0 && value.CompareTo(upper) < 0));



    // -- DateTime ------------------------------------------------------------

    [FuzzProperty]
    public bool IsInRange_on_DateTime_matches_manual_CompareTo(DateTime value, DateTime lower, DateTime upper) =>
        value.IsInRange(lower, upper)
            == (value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0);



    // -- string --------------------------------------------------------------
    // Bounds may be null (per ADR-0003 — they pass through to T.CompareTo).
    // The receiver may not — a null value is asserted separately below to
    // throw ArgumentNullException.

    [FuzzProperty]
    public Property IsBetween_on_string_matches_manual_CompareTo() =>
        Prop.ForAll<string, string, string>((value, lower, upper) =>
            value is null
                || value.IsBetween(lower, upper)
                    == (value.CompareTo(lower) > 0 && value.CompareTo(upper) < 0));



    [FuzzProperty]
    public Property IsInRange_on_string_matches_manual_CompareTo() =>
        Prop.ForAll<string, string, string>((value, lower, upper) =>
            value is null
                || value.IsInRange(lower, upper)
                    == (value.CompareTo(lower) >= 0 && value.CompareTo(upper) <= 0));



    // -- Cross-method invariants --------------------------------------------

    [FuzzProperty]
    public bool IsBetween_implies_IsInRange_on_int(int value, int lower, int upper) =>
        !value.IsBetween(lower, upper) || value.IsInRange(lower, upper);



    [FuzzProperty]
    public Property IsBetween_implies_IsInRange_on_string() =>
        Prop.ForAll<string, string, string>((value, lower, upper) =>
            value is null || !value.IsBetween(lower, upper) || value.IsInRange(lower, upper));



    // -- Null-receiver contract ---------------------------------------------

    [FuzzProperty]
    public Property IsBetween_on_null_string_throws_ArgumentNullException() =>
        Prop.ForAll<string, string>((lower, upper) =>
        {
            string? value = null;

            // Assert.Throws fails the property itself when nothing is thrown, so there
            // is no "return false" branch that a passing run can never execute.
            var exception = Assert.Throws<ArgumentNullException>
            (
                () => value!.IsBetween(lower, upper)
            );

            return string.Equals(exception.ParamName, nameof(value), StringComparison.Ordinal);
        });



    [FuzzProperty]
    public Property IsInRange_on_null_string_throws_ArgumentNullException() =>
        Prop.ForAll<string, string>((lower, upper) =>
        {
            string? value = null;

            // Assert.Throws fails the property itself when nothing is thrown, so there
            // is no "return false" branch that a passing run can never execute.
            var exception = Assert.Throws<ArgumentNullException>
            (
                () => value!.IsInRange(lower, upper)
            );

            return string.Equals(exception.ParamName, nameof(value), StringComparison.Ordinal);
        });
}
