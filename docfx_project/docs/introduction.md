# Introduction

Welcome to Wolfgang.Extensions.IComparable!

## Overview

**Wolfgang.Extensions.IComparable** is a lightweight .NET library that provides extension methods for types implementing the `IComparable<T>` interface. It simplifies common comparison operations, making range checks read the way you'd say them — `value.IsBetween(low, high)` instead of `value.CompareTo(low) > 0 && value.CompareTo(high) < 0`.

## Why Use This Library?

Without the library, range checks are verbose and easy to get wrong:

```csharp
// Without Wolfgang.Extensions.IComparable
if (value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0)
{
    // Value is in range
}

// With Wolfgang.Extensions.IComparable
if (value.IsInRange(min, max))
{
    // Value is in range — much clearer
}
```

## Key Features

- **`IsBetween`** — strict (exclusive) range check: `value > lower && value < upper`
- **`IsInRange`** — inclusive range check: `value >= lower && value <= upper`
- **Type-safe** — works with any type that implements `IComparable<T>`
- **Zero dependencies** — pure .NET Standard 2.0 library
- **Multi-target** — runs on .NET Framework 4.6.2+, .NET Standard 2.0+, and .NET 8.0+

## Available Extension Methods

### IsBetween

Determines if a value is **strictly between** two bounds (exclusive):

```csharp
int value = 5;
bool result = value.IsBetween(1, 10); // true — 5 > 1 AND 5 < 10
```

### IsInRange

Determines if a value is **within the range** of two bounds (inclusive):

```csharp
int value = 5;
bool result = value.IsInRange(5, 10); // true — 5 >= 5 AND 5 <= 10
```

## Supported Types

Any type implementing `IComparable<T>` works, including:

- Numeric types — `int`, `double`, `decimal`, `float`, `long`, etc.
- Date/time types — `DateTime`, `DateTimeOffset`, `TimeSpan`
- `string` and `char`
- Custom types that implement `IComparable<T>`

## Getting Help

If you need help with Wolfgang.Extensions.IComparable, please:

- Check the [Getting Started](getting-started.md) guide
- Review the [API Reference](https://chris-wolfgang.github.io/IComparable-Extensions/versions/latest/api/Wolfgang.Extensions.IComparable.html)
- Visit the [GitHub repository](https://github.com/Chris-Wolfgang/IComparable-Extensions)
- Open an issue on [GitHub Issues](https://github.com/Chris-Wolfgang/IComparable-Extensions/issues)
