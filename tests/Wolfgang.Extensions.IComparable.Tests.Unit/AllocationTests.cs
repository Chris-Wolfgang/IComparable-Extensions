#if NET5_0_OR_GREATER
using System;

namespace Wolfgang.Extensions.IComparable.Tests.Unit;

/// <summary>
/// Verifies the value-type hot paths through <see cref="IComparableExtensions.IsBetween{T}"/>
/// and <see cref="IComparableExtensions.IsInRange{T}"/> allocate zero managed bytes. The
/// generic constraint <c>where T : IComparable&lt;T&gt;</c> allows non-boxing calls to
/// <c>T.CompareTo(T)</c> for value types; if a future change ever accidentally boxes T
/// (for example by widening the constraint to non-generic <c>IComparable</c>, or by
/// adding an intermediate object cast) these tests fail loudly instead of degrading
/// runtime allocation profiles silently.
///
/// Guarded on NET5_0_OR_GREATER because <see cref="GC.GetAllocatedBytesForCurrentThread"/>
/// is only available from .NET 5 onwards; older TFMs skip this class at compile time.
/// </summary>
public class AllocationTests
{
    private const int Iterations = 10_000;


    [Fact]
    public void IsBetween_on_int_allocates_zero_bytes()
    {
        AssertZeroAllocation(() =>
        {
            for (var i = 0; i < Iterations; i++)
            {
                _ = 5.IsBetween(1, 10);
            }
        });
    }


    [Fact]
    public void IsInRange_on_int_allocates_zero_bytes()
    {
        AssertZeroAllocation(() =>
        {
            for (var i = 0; i < Iterations; i++)
            {
                _ = 5.IsInRange(1, 10);
            }
        });
    }


    [Fact]
    public void IsBetween_on_DateTime_allocates_zero_bytes()
    {
        var value = new DateTime(2026, 5, 15);
        var lower = new DateTime(2026, 4, 1);
        var upper = new DateTime(2026, 6, 30);
        AssertZeroAllocation(() =>
        {
            for (var i = 0; i < Iterations; i++)
            {
                _ = value.IsBetween(lower, upper);
            }
        });
    }


    [Fact]
    public void IsInRange_on_DateTime_allocates_zero_bytes()
    {
        var value = new DateTime(2026, 5, 15);
        var lower = new DateTime(2026, 4, 1);
        var upper = new DateTime(2026, 6, 30);
        AssertZeroAllocation(() =>
        {
            for (var i = 0; i < Iterations; i++)
            {
                _ = value.IsInRange(lower, upper);
            }
        });
    }


    [Fact]
    public void IsBetween_on_long_allocates_zero_bytes()
    {
        AssertZeroAllocation(() =>
        {
            for (var i = 0; i < Iterations; i++)
            {
                _ = 5L.IsBetween(1L, 10L);
            }
        });
    }


    [Fact]
    public void IsBetween_on_double_allocates_zero_bytes()
    {
        AssertZeroAllocation(() =>
        {
            for (var i = 0; i < Iterations; i++)
            {
                _ = 5.0.IsBetween(1.0, 10.0);
            }
        });
    }


    private static void AssertZeroAllocation(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        // Warm-up run — JITs the generic method for T = int / DateTime / etc.,
        // and lets any first-call setup work happen outside the measurement window.
        action();

        var before = GC.GetAllocatedBytesForCurrentThread();
        action();
        var after = GC.GetAllocatedBytesForCurrentThread();

        var allocated = after - before;
        Assert.True
        (
            allocated == 0,
            $"Expected 0 bytes allocated on value-type hot path; observed {allocated} bytes over {Iterations} iterations."
        );
    }
}
#endif
