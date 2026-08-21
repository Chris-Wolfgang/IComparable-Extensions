# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added

### Changed

### Deprecated

### Removed

### Fixed

### Security

## [1.1.2] - 2026-08-21

Infrastructure and quality-hardening round. No public API or runtime
behaviour change vs v1.1.1 — the shipped `Wolfgang.Extensions.IComparable.dll`
is functionally identical. All the value in this release is in
build-time gates, test coverage, and release-path security posture.

### Added

- **Architecture Decision Records** — `docs/adr/` bootstrapped with a
  Nygard template, an index, and 5 ADRs capturing non-obvious
  decisions (two methods vs. one bool flag; pinned `AssemblyVersion`;
  gated `PublicApiAnalyzers`; explicit `using System;` instead of
  `ImplicitUsings`; culture-sensitivity follows `T.CompareTo`).
- **Migration guide scaffold** — `docs/migrations/` with a template
  and process convention (migration guide lands in the same PR as
  a MAJOR bump, linked from the GitHub Release).
- **XML `<example>` blocks** on `IsBetween` and `IsInRange`, plus a
  Roslyn-hosted `.Tests.DocExamples` project that compiles every
  `<example><code>` block against the real library — doc drift now
  fails the build.
- **Globalization / CultureInfo invariance matrix** — 37 test cases
  under Invariant, en-US, tr-TR, de-DE, zh-CN, ar-SA, ja-JP, plus a
  dedicated Turkish dotted-I trap test.
- **Allocation-free hot-path verification** — 6 tests asserting
  `IsBetween` / `IsInRange` on int, long, double, DateTime allocate
  0 bytes per call.
- **Property-based fuzz tests** (`.Tests.Fuzz`) — FsCheck properties
  verifying the extensions agree with a hand-written `T.CompareTo`
  chain for every random input, across int / long / double / DateTime
  / string.
- **AOT smoke consumer** (`.Tests.AotSmoke`) — a `PublishAot=true` +
  `PublishTrimmed=true` console app that exercises every public method
  and gets published + executed on Linux in CI.
- **`PackageValidation` gate** — `dotnet pack` now fails on a
  binary-breaking change vs the last-published version (baseline
  currently 1.1.1). Intentional breaks require an explicit
  `CompatibilitySuppressions.xml` waiver.
- **Release path & compromise scope appendix** in `SECURITY.md`.
- **Test-assembly coverage instrumentation** — `IncludeTestAssembly`
  in `coverlet.runsettings` makes test-code coverage first-class.
- **New CI workflows**:
    - `aot-smoke.yaml` — Trim/AOT compatibility gate on Linux.
    - `reproducible-build.yaml` — twice-build sha256 diff.
    - `pr-benchmarks.yaml` — BDN delta table as PR comment (advisory).
    - `pull_request` trigger added to `stryker.yaml` — mutation-score
      gate on PRs that touch src / tests (break threshold at 80).

### Changed

- **Release path migrated to NuGet Trusted Publishing (OIDC)** —
  `release.yaml` now uses `NuGet/login@v1` to mint an ephemeral
  push key per run via GitHub's OIDC token. The long-lived
  `NUGET_API_KEY` secret is no longer referenced.
- **`ImplicitUsings` disabled on the src project** — `using System;`
  is uniformly required across all four target frameworks (net462,
  netstandard2.0, net8.0, net10.0). Eliminates a per-TFM analyzer
  mismatch. No compiled-output change.
- **`PublicApiAnalyzers` gate** in `Directory.Build.props` — the
  package reference and its `AdditionalFiles` are now both gated on
  `Exists('PublicAPI.Shipped.txt')`. Kills a ~500-alert false-positive
  flood in projects that don't publish an API surface.
- **README** — dead-link cleanup and Supported Frameworks section
  aligned with the canonical fleet shape.

### Security

- **Trusted Publishing** (see Changed) removes the long-lived
  `NUGET_API_KEY` from the release-path dependency chain. Every
  release now runs against a scoped ephemeral credential from GitHub
  OIDC.

## [1.1.1] - 2026-06-01

Canonical maintenance round + binding-stability fix. No public API or
runtime behavior change vs v1.1.0.

### Added

- **D8** — `verify-docs-build` job in `release.yaml` runs DocFX during
  the release pipeline before the NuGet push, so a docs build failure
  now blocks the package from shipping.
- **D8** — docs site version picker assets
  (`docfx_project/public/version-picker.js`,
  `docfx_project/versions.json`,
  `docs/DOCFX-VERSION-PICKER.md`).
- **A1** — `PublicApiAnalyzers` scaffolding (analyzers activate when
  `PublicAPI.Shipped.txt` / `PublicAPI.Unshipped.txt` are present
  alongside the csproj).
- **CI3** — canonical NuGet package metadata: `Authors`, `Copyright`,
  `RepositoryType`, SourceLink, snupkg symbol packages, deterministic
  CI build flag, and `EmbedUntrackedSources` hoisted to
  `Directory.Build.props`.
- **T3** — Stryker mutation-testing workflow (`stryker.yaml`).
- **T1** — coverage report published to docs site.
- **S1** — CodeQL `security-extended` query pack.
- **D6** — versions.json preservation guard on the docs deploy.

### Changed

- **C1** — fleet-wide template-drift sync: workflow files (`pr.yaml`,
  `release.yaml`, `docfx.yaml`, `codeql.yaml`,
  `build-all-versions.yaml`, `stryker.yaml`), `.editorconfig`,
  `BannedSymbols.txt`, `Directory.Build.props`, and per-context
  `tests/Directory.Build.props` consolidated to the canonical baseline.
- **Nullable** — `<Nullable>enable</Nullable>` consolidated into
  `Directory.Build.props` (was per-csproj); per-project opt-out via
  override still supported.
- **CI2** — Dependabot `github-actions` ecosystem added.
- **D3** — repo scripts hardened (`Setup-Labels.ps1`,
  `Fix-BranchRuleset.ps1`).
- `github/codeql-action/init` and `analyze` bumped v3 → v4 (Node.js
  20 → 24 deprecation).
- Analyzer/test-tool version bumps folded in from the in-flight
  Dependabot PRs:
  - `Meziantou.Analyzer` 3.0.85 → 3.0.98
  - `SonarAnalyzer.CSharp` 10.25.0 → 10.27.0
  - `coverlet.collector` 10.0.0 → 10.0.1

### Fixed

- **C4** — restored explicit `<AssemblyVersion>1.0.0.0</AssemblyVersion>`
  and added a prerelease-safe `<FileVersion>` (regex-strip property
  function) to the src csproj. The original C4 fanout had dropped
  these on the rationale that the hardcoded values were "stale"
  relative to released package versions — but that staleness was the
  correct binding-stability behaviour for libraries that ship a
  `net462` TFM. Without an explicit pin, SDK-derived `AssemblyVersion`
  would change on every minor/patch release, breaking .NET Framework
  consumers without a binding redirect. (See DateTime-Extensions v1.3.1
  for the post-mortem on what happens when this regression reaches a
  release.)

[Unreleased]: https://github.com/Chris-Wolfgang/IComparable-Extensions/compare/v1.1.1...HEAD
[1.1.1]: https://github.com/Chris-Wolfgang/IComparable-Extensions/compare/v1.1.0...v1.1.1
[1.1.0]: https://github.com/Chris-Wolfgang/IComparable-Extensions/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/Chris-Wolfgang/IComparable-Extensions/releases/tag/v1.0.0
