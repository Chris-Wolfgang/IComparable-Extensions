# 0005. Culture-sensitivity of `IsBetween` / `IsInRange` follows `T.CompareTo`; the library adds no culture layer

- **Date:** 2026-08-20
- **Status:** Accepted

## Context

Range checks over generic `T : IComparable<T>` need a policy for how they behave under non-`en-US` cultures. `string.CompareTo(string)` uses `CultureInfo.CurrentCulture`, so Turkish's dotted/dotless-I split, German ß, and simplified-Chinese collation all change the answer to `"foo".CompareTo("bar")` at runtime depending on `Thread.CurrentThread.CurrentCulture`.

Three options for the library:

1. **Force `CurrentCulture` semantics** — just call `T.CompareTo` as-is (implicit today).
2. **Force `Ordinal` semantics** — reroute `T = string` to `String.Compare(..., Ordinal)` behind the scenes for safety.
3. **Offer both**, with a `StringComparison` parameter or a second overload.

Option 2 changes the answer silently — a consumer that previously relied on locale-aware ordering would suddenly get ordinal ordering. Option 3 doubles the API surface for a single concrete `T` and forces a design decision at every call site.

## Decision

We will use **option 1**: `IsBetween<T>` and `IsInRange<T>` call `T.CompareTo(T)` directly. Whatever culture-sensitivity `T.CompareTo` has is what the extension method has. No wrapping, no rerouting, no extra parameter.

## Consequences

- **The library itself is culture-invariant** — its behavior does not depend on `CurrentCulture` in any way that `T.CompareTo` doesn't already. The `CultureInvarianceTests` matrix in the test suite asserts this by running the same assertions under `en-US`, `tr-TR`, `de-DE`, `zh-CN`, `ar-SA`, `ja-JP` and verifying every call agrees with the direct `T.CompareTo` answer under that same culture.
- **When `T = string`, callers inherit `String.CompareTo`'s culture-sensitivity.** This is the standard .NET convention (`SortedSet<string>`, `List<string>.Sort()`, `Array.Sort` on `string[]` all behave the same way). A caller that needs ordinal semantics can wrap their strings in a comparer-aware container or use a separate string extension library.
- **Allowlist of intentionally culture-sensitive public methods:** none *in this library*. The culture-sensitivity that arises with `T = string` (and any other type whose `CompareTo` implementation is culture-aware) is a **propagation** from the contract of that type, not something the library introduces.
- **No new overload with a `StringComparison`.** If demand appears we would add a separate `IsBetween(this string, string, string, StringComparison)` — additive, opt-in — rather than change the behavior of the generic form.
