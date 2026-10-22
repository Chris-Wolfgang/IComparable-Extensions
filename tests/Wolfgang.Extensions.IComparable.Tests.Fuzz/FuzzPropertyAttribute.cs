using FsCheck.Xunit;

namespace Wolfgang.Extensions.IComparable.Tests.Fuzz;

/// <summary>
/// An FsCheck <see cref="PropertyAttribute"/> whose case count comes from the
/// <c>FSCHECK_MAX_TESTS</c> environment variable. Unset (local runs and PR CI) it is
/// FsCheck's default of 100 cases per property; the weekly <c>fuzz.yaml</c> run sets it to
/// 100,000 (#157). A failing property prints a <c>Replay</c> seed; pass it as
/// <c>[FuzzProperty(Replay = "...")]</c> to re-run the shrunk case deterministically.
/// </summary>
public sealed class FuzzPropertyAttribute : PropertyAttribute
{
    /// <summary>The case count used when <c>FSCHECK_MAX_TESTS</c> is unset or invalid.</summary>
    public const int DefaultMaxTest = 100;



    /// <summary>Initializes the attribute with the case count from the environment.</summary>
    public FuzzPropertyAttribute()
    {
        MaxTest = ResolveMaxTest(Environment.GetEnvironmentVariable("FSCHECK_MAX_TESTS"));
    }



    /// <summary>
    /// Parses <paramref name="value"/> as a positive case count, falling back to
    /// <see cref="DefaultMaxTest"/>.
    /// </summary>
    internal static int ResolveMaxTest(string? value) =>
        int.TryParse(value, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var count) && count > 0
            ? count
            : DefaultMaxTest;
}
