using System.Linq;
using FsCheck;
using FsCheck.Xunit;

namespace Wolfgang.Extensions.IComparable.Tests.Fuzz;

/// <summary>
/// Property-based fuzz tests for the comparison, <c>Clamp</c>, <c>Min</c>/<c>Max</c>,
/// <c>NearestTo</c> and <c>WhereBetween</c>/<c>WhereInRange</c> extensions. As in
/// <see cref="IComparableExtensionsProperties"/>, each property checks the method against a
/// reference written with plain <c>CompareTo</c> or LINQ, so any added or lost semantic shows up
/// as a counter-example. Bounds are put in order with <see cref="Math.Min(int, int)"/> /
/// <see cref="Math.Max(int, int)"/> where a method requires it, so no property has a branch that
/// a given run might never take.
/// </summary>
public class ComparisonClampMinMaxProperties
{
    // -- IsGreaterThan / IsLessThan / ...OrEqualTo ------------------------------

    [Property]
    public bool IsGreaterThan_on_int_matches_manual_CompareTo(int value, int other) =>
        value.IsGreaterThan(other) == (value.CompareTo(other) > 0);


    [Property]
    public bool IsLessThan_on_int_matches_manual_CompareTo(int value, int other) =>
        value.IsLessThan(other) == (value.CompareTo(other) < 0);


    [Property]
    public bool IsGreaterThanOrEqualTo_on_int_matches_manual_CompareTo(int value, int other) =>
        value.IsGreaterThanOrEqualTo(other) == (value.CompareTo(other) >= 0);


    [Property]
    public bool IsLessThanOrEqualTo_on_int_matches_manual_CompareTo(int value, int other) =>
        value.IsLessThanOrEqualTo(other) == (value.CompareTo(other) <= 0);


    [Property]
    public Property IsGreaterThan_on_string_matches_manual_CompareTo() =>
        Prop.ForAll<NonNull<string>, string>((value, other) =>
            value.Get.IsGreaterThan(other) == (value.Get.CompareTo(other) > 0));


    // -- Clamp -----------------------------------------------------------------

    [Property]
    public bool Clamp_on_int_matches_Math_Min_Max(int value, int a, int b)
    {
        var min = Math.Min(a, b);
        var max = Math.Max(a, b);

        return value.Clamp(min, max) == Math.Min(Math.Max(value, min), max);
    }


    [Property]
    public bool Clamp_on_int_result_is_in_range(int value, int a, int b)
    {
        var min = Math.Min(a, b);
        var max = Math.Max(a, b);

        return value.Clamp(min, max).IsInRange(min, max);
    }


    // -- Min / Max ---------------------------------------------------------------

    [Property]
    public bool Min_and_Max_on_int_match_Math(int value, int other) =>
        value.Min(other) == Math.Min(value, other)
            && value.Max(other) == Math.Max(value, other);


    [Property]
    public bool Min_and_Max_with_params_on_int_match_Enumerable(int value, int other, int[] others)
    {
        var all = new[] { value, other }.Concat(others).ToArray();

        return value.Min(other, others) == all.Min()
            && value.Max(other, others) == all.Max();
    }


    [Property]
    public Property Min_and_Max_over_a_sequence_on_int_match_Enumerable() =>
        Prop.ForAll<int, NonEmptyArray<int>>((value, others) =>
        {
            var all = others.Get.Concat(new[] { value }).ToArray();

            return value.Min(others.Get.AsEnumerable()) == all.Min()
                && value.Max(others.Get.AsEnumerable()) == all.Max();
        });


    // -- NearestTo ---------------------------------------------------------------

    [Property]
    public Property NearestTo_on_int_returns_a_candidate_at_the_smallest_distance() =>
        Prop.ForAll<int, NonEmptyArray<int>>((value, candidates) =>
        {
            var nearest = value.NearestTo(candidates.Get, Distance);

            return candidates.Get.Contains(nearest)
                && Distance(value, nearest) == candidates.Get.Min(c => Distance(value, c));
        });


    // -- WhereBetween / WhereInRange ---------------------------------------------

    [Property]
    public Property WhereBetween_on_int_matches_IsBetween() =>
        Prop.ForAll<NonEmptyArray<int>, int, int>((values, lower, upper) =>
            values.Get.AsQueryable().WhereBetween(x => x, lower, upper)
                .SequenceEqual(values.Get.Where(x => x.IsBetween(lower, upper))));


    [Property]
    public Property WhereInRange_on_int_matches_IsInRange() =>
        Prop.ForAll<NonEmptyArray<int>, int, int>((values, lower, upper) =>
            values.Get.AsQueryable().WhereInRange(x => x, lower, upper)
                .SequenceEqual(values.Get.Where(x => x.IsInRange(lower, upper))));


    [Property]
    public Property WhereBetween_on_string_matches_IsBetween() =>
        Prop.ForAll<NonEmptyArray<NonNull<string>>, string, string>((values, lower, upper) =>
        {
            var strings = values.Get.Select(s => s.Get).ToArray();

            return strings.AsQueryable().WhereBetween(s => s, lower, upper)
                .SequenceEqual(strings.Where(s => s.IsBetween(lower, upper)));
        });



    private static long Distance(int a, int b) => Math.Abs((long)a - b);
}
