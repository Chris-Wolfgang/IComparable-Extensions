using BenchmarkDotNet.Attributes;
using Wolfgang.Extensions.IComparable;

namespace Wolfgang.Extensions.IComparable.Benchmarks;

/// <summary>
/// Compares <see cref="IComparableExtensions.IsInRange{T}(T, T, T)"/> against
/// the hand-written CompareTo idiom. Inclusive contract → checks both boundary
/// cases (true) and a strictly-inside case for value-type (int) and
/// reference-type (string) paths.
/// </summary>
[MemoryDiagnoser]
[RankColumn]
public class IsInRangeBenchmarks
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
        _intValueInside.CompareTo(_intLower) >= 0 &&
        _intValueInside.CompareTo(_intUpper) <= 0;

    [Benchmark]
    public bool IsInRange_Int_Inside() =>
        _intValueInside.IsInRange(_intLower, _intUpper);

    // ----- int — value equal to lower bound (inclusive contract → true) -----

    [Benchmark]
    public bool Manual_Int_AtLower() =>
        _intValueAtLower.CompareTo(_intLower) >= 0 &&
        _intValueAtLower.CompareTo(_intUpper) <= 0;

    [Benchmark]
    public bool IsInRange_Int_AtLower() =>
        _intValueAtLower.IsInRange(_intLower, _intUpper);

    // ----- string — reference-type CompareTo path -----

    [Benchmark]
    public bool Manual_String_Inside() =>
        _stringValueInside.CompareTo(_stringLower) >= 0 &&
        _stringValueInside.CompareTo(_stringUpper) <= 0;

    [Benchmark]
    public bool IsInRange_String_Inside() =>
        _stringValueInside.IsInRange(_stringLower, _stringUpper);

    [Benchmark]
    public bool Manual_String_AtLower() =>
        _stringValueAtLower.CompareTo(_stringLower) >= 0 &&
        _stringValueAtLower.CompareTo(_stringUpper) <= 0;

    [Benchmark]
    public bool IsInRange_String_AtLower() =>
        _stringValueAtLower.IsInRange(_stringLower, _stringUpper);
}
