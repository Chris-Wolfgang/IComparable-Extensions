type: feature

Add `Min` and `Max` for any `IComparable<T>`: two-value, several-value (`value.Min(a, b, c)`) and sequence (`value.Max(list)`) overloads; null candidates are ignored and ties return the earliest value.
