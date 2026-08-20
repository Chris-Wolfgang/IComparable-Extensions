# 0004. Explicit `using System;` in src instead of `<ImplicitUsings>enable</ImplicitUsings>`

- **Date:** 2026-08-20
- **Status:** Accepted

## Context

The src project targets four frameworks: `net462`, `netstandard2.0`, `net8.0`, and `net10.0`. `<ImplicitUsings>enable</ImplicitUsings>` only takes effect on `net6.0` and later — on `net462` and `netstandard2.0` the setting is silently ignored, so `using System;` remains required to resolve `IComparable<T>` and `ArgumentNullException`.

A previous configuration enabled ImplicitUsings conditionally, only for the modern TFMs. The result was a per-TFM mismatch: the explicit `using System;` in `IComparableExtensions.cs` was required on `net462` / `netstandard2.0` and redundant on `net8.0` / `net10.0`. The `RedundantUsingDirective` analyzer (which analyzes per compilation) reported it as redundant on the modern TFMs, then the aggregated SARIF surfaced it as a single alert with no way to satisfy both TFM views. The only way to silence it was a `// ReSharper disable once` suppression comment — which converted "code right, analyzer confused by cross-TFM aggregation" into a permanent noise-suppression instead of a fix.

## Decision

We will leave `<ImplicitUsings>` off on the src project and require an explicit `using System;` (and any other required `System.*` usings) in every source file. The requirement is uniform across every TFM: the file needs the using or it does not compile.

## Consequences

- The `RedundantUsingDirective` analyzer agrees with the code on every TFM — no per-TFM mismatch, no aggregated-SARIF alert, no suppression comment.
- Slightly more verbose per file — every src file that touches `System` needs an explicit using. This library is tiny; the cost is negligible. Larger projects would trade this off differently.
- Any new src file must add its own `using System;` etc. explicitly, like `netstandard2.0`-era code — new contributors coming from `net8.0`-only projects may be briefly surprised.
- The setting change is build-time-only. The compiled DLL is byte-identical whether `ImplicitUsings` is on or off, provided the source file has the explicit usings it needs. No consumer-visible impact.
- Test / benchmark / example projects target a single modern TFM and keep `<ImplicitUsings>enable</ImplicitUsings>` — the mismatch problem does not apply there.
