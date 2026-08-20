# 0001. Two methods (`IsBetween` exclusive, `IsInRange` inclusive) instead of one method with a bound-inclusivity flag

- **Date:** 2026-08-20
- **Status:** Accepted

## Context

Range-check APIs come in two shapes:

1. **One method + a flag:** `value.IsBetween(low, high, inclusive: true)`.
2. **Two named methods:** `value.IsBetween(low, high)` (exclusive) plus `value.IsInRange(low, high)` (inclusive).

Shape 1 is more compact at the type-declaration level (one method, two overloads at most). Shape 2 doubles the public surface but makes the *bound semantics* part of the method name.

The failure mode we cared about was call-site readability. `value.IsBetween(low, high, true)` at a call site tells the reader nothing about whether the bounds are inclusive — they have to hover the parameter list or read the docs. Named-argument style helps (`inclusive: true`) but is not enforceable, and code review does not reliably catch a bare `true`/`false` as a semantic error. The bound semantics are exactly the kind of thing that must be obvious at the call site, because getting them wrong silently changes the answer on the boundary case.

## Decision

We will expose two separately-named methods: `IsBetween` for strict / exclusive bounds and `IsInRange` for inclusive bounds. The bound semantics are baked into the name, not into a parameter.

## Consequences

- Call sites read like English: `score.IsBetween(70, 80)` and `today.IsInRange(quarterStart, quarterEnd)` each convey their own bound semantics without a bool parameter or a hover-doc trip.
- No default-value trap — there is no "which bound-inclusivity did we default to?" question because there is no default to get wrong.
- Public surface is two methods per `T` instead of one; the doc/PublicAPI baseline is slightly larger.
- Future range-related extensions (e.g. `IsWithin` for tolerance-based checks, a hypothetical mixed-inclusivity variant) should follow the same "bound semantics in the name" convention rather than reopening the flag debate.
