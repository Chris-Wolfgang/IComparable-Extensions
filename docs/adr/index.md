# Architecture Decision Records

Short, dated records of non-obvious choices in this library — the *why* behind decisions that would otherwise get re-derived (often poorly) months after the PR that introduced them.

Format is [Michael Nygard's ADR](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions) (see [TEMPLATE.md](TEMPLATE.md)). New ADRs land alongside the PR that introduces the corresponding decision and are numbered sequentially, four digits, zero-padded.

## Index

| # | Title | Status |
|---|---|---|
| [0001](0001-two-methods-not-one-with-a-flag.md) | Two methods (`IsBetween` exclusive, `IsInRange` inclusive) instead of one method with a bound-inclusivity flag | Accepted |
| [0002](0002-pin-assemblyversion-at-1-0-0-0.md) | Pin `AssemblyVersion` at `1.0.0.0`; bump only on a breaking API change | Accepted |
| [0003](0003-publicapianalyzers-opt-in-per-project.md) | `PublicApiAnalyzers` is opt-in per project via `Exists('PublicAPI.Shipped.txt')` | Accepted |
| [0004](0004-explicit-using-system-instead-of-implicit-usings.md) | Explicit `using System;` in src instead of `<ImplicitUsings>enable</ImplicitUsings>` | Accepted |
| [0005](0005-culture-sensitivity-follows-t-compareto.md) | Culture-sensitivity of `IsBetween` / `IsInRange` follows `T.CompareTo`; the library adds no culture layer | Accepted |
