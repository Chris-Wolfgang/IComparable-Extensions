using System;
using Wolfgang.Extensions.IComparable;

// AOT smoke consumer. Exercises every public method on every representative
// T (int, DateTime, string) so the trimmer can't drop the required generic
// instantiations, and a startup crash on any code path fails the workflow's
// `Run AOT-published binary` step.
//
// Assertions are checked at runtime and print the failing case before
// throwing so a CI log shows exactly what broke.

Check("IsBetween on int inside",   5.IsBetween(1, 10),                  expected: true);
Check("IsBetween on int at lower", 1.IsBetween(1, 10),                  expected: false);
Check("IsBetween on int at upper", 10.IsBetween(1, 10),                 expected: false);

Check("IsInRange on int inside",   5.IsInRange(1, 10),                  expected: true);
Check("IsInRange on int at lower", 1.IsInRange(1, 10),                  expected: true);
Check("IsInRange on int at upper", 10.IsInRange(1, 10),                 expected: true);

var quarterStart = new DateTime(2026, 4, 1);
var quarterEnd = new DateTime(2026, 6, 30);
var midQuarter = new DateTime(2026, 5, 15);
Check("IsInRange on DateTime inside", midQuarter.IsInRange(quarterStart, quarterEnd), expected: true);
Check("IsInRange on DateTime at lower", quarterStart.IsInRange(quarterStart, quarterEnd), expected: true);

Check("IsBetween on string",  "m".IsBetween("a", "z"),                  expected: true);
Check("IsInRange on string",  "a".IsInRange("a", "z"),                  expected: true);

// Argument-null path — the constructor of ArgumentNullException itself is a
// classic AOT / trim casualty. Force it once so a broken exception path
// crashes the smoke, not a downstream consumer.
try
{
    string? nullish = null;
    _ = nullish!.IsBetween("a", "z");
    throw new InvalidOperationException("expected ArgumentNullException from null value");
}
catch (ArgumentNullException)
{
    // expected
}

Console.WriteLine("AOT smoke OK.");
return 0;


static void Check(string label, bool actual, bool expected)
{
    if (actual == expected) return;
    Console.Error.WriteLine($"AOT smoke FAILED: {label} — expected {expected}, got {actual}");
    Environment.Exit(1);
}
