# Copilot Coding Agent Instructions

## Repository Summary

**Wolfgang.Extensions.IComparable** is a small, focused .NET library that ships extension methods (`IsBetween<T>`, `IsInRange<T>`) for any type implementing `IComparable<T>`. It is a published NuGet package (`Wolfgang.Extensions.IComparable`), multi-targeted across .NET Framework and modern .NET.

**Repository Type**: Production library (NuGet-published)
**Primary Language**: C# (file-scoped namespaces, nullable enabled, multi-line argument lists in Allman style)
**Target Frameworks (src)**: `net462;netstandard2.0;net8.0;net10.0`
**Target Frameworks (tests)**: `net462;net472;net48;net481;net5.0;net6.0;net7.0;net8.0;net9.0;net10.0`
**Public surface**: 2 generic extension methods on `IComparable<T>` (see `src/Wolfgang.Extensions.IComparable/IComparableExtensions.cs`)

## Build and Validation

### Prerequisites
- **.NET 10.0 SDK** (drives the modern targets; older TFMs build via reference assemblies that the SDK ships).
- The Release build enforces `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` via `Directory.Build.props` — any analyzer warning is a build error.

### Local commands
```bash
# Restore + build everything in Release (the only configuration that matters for CI parity)
dotnet build -c Release

# Run every TFM of the test project
dotnet test -c Release --no-build

# Build a single TFM if you're iterating fast
dotnet build src/Wolfgang.Extensions.IComparable/Wolfgang.Extensions.IComparable.csproj -c Release -f net10.0
dotnet test  tests/Wolfgang.Extensions.IComparable.Tests.Unit/Wolfgang.Extensions.IComparable.Tests.Unit.csproj -c Release -f net10.0 --no-build
```

`scripts/build-pr.ps1` replicates the Windows portion of `pr.yaml` locally — useful for verifying CI parity before pushing.

### CI

| Workflow | Purpose | Triggers on |
|---|---|---|
| `.github/workflows/pr.yaml` | Multi-stage validation: Linux + Windows + macOS, coverage gate, security scans | `pull_request_target` against `main` |
| `.github/workflows/release.yaml` | NuGet publish + docs deploy | GitHub Release published |
| `.github/workflows/docfx.yaml` | DocFX site → `gh-pages` | `workflow_call` (from `release.yaml`) and `workflow_dispatch` |
| `.github/workflows/codeql.yaml` | CodeQL security-extended scan | weekly + on PR |
| `.github/workflows/stryker.yaml` | Stryker.NET mutation testing | weekly + `workflow_dispatch` |
| `.github/workflows/build-all-versions.yaml` | Builds every released tag's docs onto `gh-pages/versions/v<n>/` | manual |

`pr.yaml` only fires for PRs targeting `main`. PRs targeting the per-repo integration branch (`vNext`) skip CI — verify multi-TFM builds locally before stacking on vNext.

## Source layout

```
root/
├── IComparable-Extensions.slnx        # Solution (XML manifest format)
├── Directory.Build.props              # Canonical analyzers, nullable, package metadata
├── src/
│   └── Wolfgang.Extensions.IComparable/
│       ├── IComparableExtensions.cs   # The two extension methods
│       └── PublicAPI.Shipped.txt      # Tracked public surface (when present)
├── tests/
│   ├── .editorconfig                  # Test-specific analyzer relaxations
│   └── Wolfgang.Extensions.IComparable.Tests.Unit/
│       └── IComparableExtensionsTests.cs
├── examples/
│   ├── Wolfgang.Extensions.IComparable.DotNet462.Example1/   # legacy MSBuild
│   └── Wolfgang.Extensions.IComparable.DotNet8.Example1/     # SDK-style
├── assets/icon.png + icon.ico         # Package assets
├── docfx_project/                     # Docs site source
├── docs/                              # Hand-authored markdown (getting-started, setup, etc.)
└── scripts/                           # PowerShell + bash helpers (build-pr, format, Setup-Labels, …)
```

## Coding conventions

These rules come from `C:\Source\GitHub\CLAUDE.md` and the repo's `.editorconfig`. The Release-gate analyzers (Roslynator, Meziantou, SonarAnalyzer, Microsoft.VisualStudio.Threading.Analyzers, AsyncFixer, BannedApiAnalyzers, PublicApiAnalyzers) catch most violations automatically.

- **File-scoped namespaces** everywhere (`namespace X;`, not `namespace X { }`).
- **Allman brace style**, braces on their own line.
- **3 blank lines between** constructors, methods, and properties.
- **Multi-line argument lists** open with `(` on its own line:
  ```csharp
  return method
  (
      arg1,
      arg2,
      arg3
  );
  ```
- **Nullable reference types enabled** via `Directory.Build.props` (`<Nullable>enable</Nullable>` conditional on SDK-style C# projects).
- **`var` for locals** when the type is obvious from the right-hand side.
- **Test naming**: `MethodUnderTest_when_condition_expected_result`.
- **Assertions**: prefer `Assert.NotNull(x)` over `Assert.True(x != null)`, `Assert.Empty(coll)` over `Assert.True(coll.Count == 0)`, etc.
- **Public XML docs** required (`<GenerateDocumentationFile>True</GenerateDocumentationFile>` is on). Missing summaries fail the Release build under TWEA unless suppressed locally.

## Public API contract

- The public surface is tracked by `Microsoft.CodeAnalysis.PublicApiAnalyzers` via `src/.../PublicAPI.Shipped.txt` and `PublicAPI.Unshipped.txt` when present. Adding a public method without updating those files fails the build with RS0016.
- **AssemblyVersion is pinned at `1.0.0.0`** for binding stability across the .NET Framework TFM. Bumping `<Version>` does not bump `AssemblyVersion`. `<FileVersion>` is auto-derived from `<Version>` via a regex-strip property function. See the v1.1.1 CHANGELOG entry for the rationale.

## Branch model

- `main` is the release branch.
- `vNext` is the per-repo integration branch for the in-flight release (folds in canonical-protected + canonical-unprotected + maintenance work). The current vNext is targeting v1.1.1.
- Feature/maintenance work stacks onto `vNext` via `stack/<topic>` branches. PRs target `vNext` (not `main`) so they don't trigger pr.yaml's main-only multi-stage validation; run `scripts/build-pr.ps1` locally instead.
- `vNext → main` is one large admin-bypass merge (the protected-files guard is expected to fire on the workflow / DBP / editorconfig drift).

## Don't

- **Don't add `<AssemblyVersion>` overrides** beyond the canonical pin — bumping it breaks binding-redirect-free upgrades for net462 consumers.
- **Don't switch `record` types in test code** without confirming net462/net47x/net48x compile — those TFMs lack `System.Runtime.CompilerServices.IsExternalInit`, which the `init` accessor needs.
- **Don't introduce `async void`** outside event handlers (`AsyncFixer` rule).
- **Don't use `string ==` for case-insensitive comparison** — `MA0006` will fail the build; use the `StringComparison`-aware overload.
- **Don't commit `*.user`, `obj/`, `bin/`, `StrykerOutput/`** — all gitignored, but worth double-checking with `git status` before committing.

## Trust these instructions

This file has been validated against the v1.1.1 repo state (June 2026). If you find these instructions incomplete or wrong, prefer asking for an updated version over guessing — most fleet conventions live in `C:\Source\GitHub\CLAUDE.md` and the repo-template canonical files.
