# Migrating from vX.y to vN.0

- **Applies to consumers of:** `Wolfgang.Extensions.IComparable` vX.y → vN.0
- **Deprecation window:** _e.g. "vX.y is still supported for critical fixes through YYYY-MM-DD; no new features"_
- **Trusted publisher / signing:** _mention any signing / OIDC changes that affect consumers' verification workflows_

## Summary

One-paragraph description of the release: what changed, who is affected, and how much work the upgrade typically is (5 minutes / an afternoon / a project). Set expectations before the reader dives into the change list.

## Breaking-change inventory

A complete, exhaustive list — a consumer should not discover a breaking change at compile time that is not listed here.

| # | What changed | Why | Symptom on upgrade |
|---|---|---|---|
| 1 | _e.g. `SomeMethod(T, T, bool)` removed_ | _e.g. superseded by `SomeMethod(T, T)` + `SomeOtherMethod(T, T)` — see ADR-XXXX_ | _e.g. compile error CS1501 "No overload takes 3 arguments"_ |

## Before / after

One code sample per breaking change. Keep the samples runnable in isolation — no invented types, no elided using directives.

### 1. `SomeMethod(T, T, bool)` removed

**Before (vX.y):**

```csharp
using Wolfgang.Extensions.IComparable;

var included = value.SomeMethod(low, high, inclusive: true);
```

**After (vN.0):**

```csharp
using Wolfgang.Extensions.IComparable;

var included = value.SomeMethodInclusive(low, high);
```

## Non-breaking additions (informational)

Not required for upgrade, but worth calling out so consumers know what's new:

- _e.g. new overload for `SomeMethod(ReadOnlySpan<T>, ...)`_

## Deprecation timeline

- **vX.y** — last version to ship the removed API.
- **vN.0** — API removed.
- **vN.0.z** — no restoration of the removed API in patch releases.
- If the previous major line receives critical fixes, name the cutoff (calendar date or event) here.

## Reference

- Release notes: <link to GitHub Release>
- Related ADRs: [ADR-XXXX](../adr/XXXX-slug.md), [ADR-YYYY](../adr/YYYY-slug.md)
- Upstream discussion: <link to design issue / PR>
