using BenchmarkDotNet.Attributes;
using Wolfgang.Extensions.IComparable;

namespace Wolfgang.Extensions.IComparable.Benchmarks;

/// <summary>
/// Compares <see cref="IComparableExtensions.IsBetween{T}(T, T, T)"/> against
/// the hand-written CompareTo idiom for value-type (int) and reference-type
/// (string) cases. Both bounds and the value are stored in fields to model
/// realistic call sites where the JIT can't constant-fold the comparison.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
public class IsBetweenBenchmarks
{
    private int _intValueInside  = 5;
    private int _intValueAtLower = 1;
    private int _intLower        = 1;
    private int _intUpper        = 10;

    private string _stringValueInside  = "m";
    private string _stringValueAtLower = "a";
    private string _stringLower        = "a";
    private string _stringUpper        = "z";

    // ----- int — value strictly inside bounds -----

    [Benchmark(Baseline = true)]
    public bool Manual_Int_Inside() =>
        _intValueInside.CompareTo(_intLower) > 0 &&
        _intValueInside.CompareTo(_intUpper) < 0;

    [Benchmark]
    public bool IsBetween_Int_Inside() =>
        _intValueInside.IsBetween(_intLower, _intUpper);

    // ----- int — value equal to lower bound (exclusive contract → false) -----

    [Benchmark]
    public bool Manual_Int_AtLower() =>
        _intValueAtLower.CompareTo(_intLower) > 0 &&
        _intValueAtLower.CompareTo(_intUpper) < 0;

    [Benchmark]
    public bool IsBetween_Int_AtLower() =>
        _intValueAtLower.IsBetween(_intLower, _intUpper);

    // ----- string — reference-type CompareTo path -----

    [Benchmark]
    public bool Manual_String_Inside() =>
        _stringValueInside.CompareTo(_stringLower) > 0 &&
        _stringValueInside.CompareTo(_stringUpper) < 0;

    [Benchmark]
    public bool IsBetween_String_Inside() =>
        _stringValueInside.IsBetween(_stringLower, _stringUpper);

    [Benchmark]
    public bool Manual_String_AtLower() =>
        _stringValueAtLower.CompareTo(_stringLower) > 0 &&
        _stringValueAtLower.CompareTo(_stringUpper) < 0;

    [Benchmark]
    public bool IsBetween_String_AtLower() =>
        _stringValueAtLower.IsBetween(_stringLower, _stringUpper);
}
