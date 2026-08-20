# 0003. `PublicApiAnalyzers` is opt-in per project via `Exists('PublicAPI.Shipped.txt')`

- **Date:** 2026-08-20
- **Status:** Accepted

## Context

The `Microsoft.CodeAnalysis.PublicApiAnalyzers` package tracks public API surface against a committed baseline (`PublicAPI.Shipped.txt` + `PublicAPI.Unshipped.txt`). It is exactly the tool we want on the src project: any accidental change to the shipped surface trips `RS0016` / `RS0017` at build time.

But an analyzer package referenced unconditionally in `Directory.Build.props` loads in **every** project in the repo — src, tests, benchmarks, examples. Tests / benchmarks / examples never have a `PublicAPI.Shipped.txt` (they do not publish an API surface), so the analyzer sees zero declared API and emits `RS0016` for **every public member**. On this repo that translated to 47 false-positive `RS0016` alerts + 4 `RS0037` alerts flooding the InspectCode SARIF, plus similar noise in every consumer's IDE.

Half-gating (analyzer loaded unconditionally, `AdditionalFiles` gated on `Exists()`) is worse than no gate — the analyzer loads, sees no baseline, and treats zero declared API as the truth.

## Decision

We will gate **both** the `PackageReference` and the `AdditionalFiles` on `Exists('PublicAPI.Shipped.txt')`, in a single `ItemGroup` in `Directory.Build.props`. A project opts in by dropping `PublicAPI.Shipped.txt` (and optionally `PublicAPI.Unshipped.txt`) into its own directory. Library projects under `src/` opt in; test / benchmark / example projects do not.

```xml
<ItemGroup Condition="Exists('PublicAPI.Shipped.txt')">
  <PackageReference Include="Microsoft.CodeAnalysis.PublicApiAnalyzers" Version="..." ... />
  <AdditionalFiles Include="PublicAPI.Shipped.txt" />
  <AdditionalFiles Include="PublicAPI.Unshipped.txt" Condition="Exists('PublicAPI.Unshipped.txt')" />
</ItemGroup>
```

## Consequences

- The src project (which ships the API) gets the full benefit of the analyzer: any unshipped public member fails the Release build via `TreatWarningsAsErrors`.
- Test / benchmark / example projects never load the analyzer, so no false-positive `RS0016` flood, no ambient IDE noise, no ceremony for contributors adding new test methods.
- To promote a new project into the "tracked public surface" set, drop the two `PublicAPI.*.txt` files in its directory. No csproj edit required, no Directory.Build.props change required.
- Discovered and applied fleet-wide in the InspectCode noise-floor sweep (this repo: PR #225). The same anti-pattern exists in any repo that uses the analyzer with an unconditional PackageReference.
