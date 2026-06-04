# Wolfgang.Extensions.IComparable

Extension methods for `IComparable<T>` that make ordering and range checks read the way you'd say them — `value.IsBetween(low, high)` instead of `value.CompareTo(low) > 0 && value.CompareTo(high) < 0`.

[![NuGet](https://img.shields.io/nuget/v/Wolfgang.Extensions.IComparable.svg?logo=nuget&label=NuGet)](https://www.nuget.org/packages/Wolfgang.Extensions.IComparable)
[![NuGet downloads](https://img.shields.io/nuget/dt/Wolfgang.Extensions.IComparable.svg?logo=nuget&label=downloads)](https://www.nuget.org/packages/Wolfgang.Extensions.IComparable)
[![PR build](https://img.shields.io/github/actions/workflow/status/Chris-Wolfgang/IComparable-Extensions/pr.yaml?event=pull_request_target&label=PR%20build&logo=github)](https://github.com/Chris-Wolfgang/IComparable-Extensions/actions/workflows/pr.yaml)
[![Release](https://img.shields.io/github/actions/workflow/status/Chris-Wolfgang/IComparable-Extensions/release.yaml?label=release&logo=github)](https://github.com/Chris-Wolfgang/IComparable-Extensions/actions/workflows/release.yaml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-Multi--Targeted-purple.svg)](https://dotnet.microsoft.com/)
[![GitHub](https://img.shields.io/badge/GitHub-Repository-181717?logo=github)](https://github.com/Chris-Wolfgang/IComparable-Extensions)

---

## 📦 Installation

```bash
dotnet add package Wolfgang.Extensions.IComparable
```

**NuGet Package:** [Wolfgang.Extensions.IComparable](https://www.nuget.org/packages/Wolfgang.Extensions.IComparable)

---

## 📄 License

This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

## 📚 Documentation

- **GitHub Repository:** [https://github.com/Chris-Wolfgang/IComparable-Extensions](https://github.com/Chris-Wolfgang/IComparable-Extensions)
- **API Documentation:** https://Chris-Wolfgang.github.io/IComparable-Extensions/
- **CHANGELOG:** [CHANGELOG.md](CHANGELOG.md)
- **Contributing Guide:** [CONTRIBUTING.md](CONTRIBUTING.md)

---

## ✨ Methods

All methods are generic extension methods on `T where T : IComparable<T>`. Both throw `ArgumentNullException` when `value` is `null`.

| Method | Bound semantics | Returns `true` when |
|---|---|---|
| `value.IsBetween(lowerBound, upperBound)` | **Exclusive** on both ends | `lowerBound < value < upperBound` |
| `value.IsInRange(lowerBound, upperBound)` | **Inclusive** on both ends | `lowerBound ≤ value ≤ upperBound` |

The distinction matters when `value` exactly equals one of the bounds: `IsBetween` returns `false`, `IsInRange` returns `true`. Pick the one whose contract you want.

---

## 🚀 Usage

### `IsBetween` — strict bounds

```csharp
using Wolfgang.Extensions.IComparable;

int score = 75;
bool inOpenRange = score.IsBetween(70, 80);   // true  — 70 < 75 < 80
bool atLower     = 70.IsBetween(70, 80);      // false — 70 is NOT > 70
bool atUpper     = 80.IsBetween(70, 80);      // false — 80 is NOT < 80
```

### `IsInRange` — inclusive bounds

```csharp
using Wolfgang.Extensions.IComparable;

DateTime today = DateTime.UtcNow.Date;
DateTime quarterStart = new DateTime(2026, 4, 1);
DateTime quarterEnd   = new DateTime(2026, 6, 30);

bool inQ2 = today.IsInRange(quarterStart, quarterEnd);   // true on any Q2 date,
                                                          // including the boundary days
```

### Works with any `IComparable<T>`

```csharp
using Wolfgang.Extensions.IComparable;

// string
"banana".IsInRange("apple", "cherry");   // true — lexicographic ordering

// custom type — just implement IComparable<T>
public record Money(decimal Amount, string Currency) : IComparable<Money>
{
    public int CompareTo(Money? other) =>
        // Per the IComparable<T> contract, any non-null value is
        // greater than null. Returning Amount.CompareTo(other?.Amount ?? 0m)
        // would be wrong for negative Amount — a negative Money would
        // compare as "less than" null instead of greater.
        other is null ? 1 : Amount.CompareTo(other.Amount);
}

var price = new Money(19.99m, "USD");
price.IsBetween(new Money(10m, "USD"), new Money(50m, "USD"));   // true
```

### Null safety

```csharp
string? maybeNull = null;
// The `!` keeps the example warning-free under <Nullable>enable</Nullable>;
// IsBetween's T is non-null, so without it the compiler complains. The
// runtime behaviour we're demonstrating is still the same:
maybeNull!.IsBetween("a", "z");   // throws ArgumentNullException("value")
```

---

## 🎯 Target frameworks

Multi-targeted to keep both modern .NET projects and long-tail .NET Framework consumers covered:

- `net462`
- `netstandard2.0`
- `net8.0`
- `net10.0`
