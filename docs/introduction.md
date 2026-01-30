# Introduction

## Overview

**Wolfgang.Extensions.IComparable** is a lightweight .NET library that provides powerful extension methods for types implementing the `IComparable<T>` interface. This library simplifies common comparison operations, making your code more readable and expressive.

## What is IComparable?

The `IComparable<T>` interface is a fundamental part of .NET that allows objects to be compared for ordering purposes. Types that implement this interface can be sorted and compared using standardized methods.

## Why Use This Library?

When working with comparable types in C#, you often need to check if a value falls within a specific range. Without this library, such checks can be verbose and error-prone:

```csharp
// Without Wolfgang.Extensions.IComparable
if (value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0)
{
    // Value is in range
}

// With Wolfgang.Extensions.IComparable
if (value.IsInRange(min, max))
{
    // Value is in range - much clearer!
}
```

## Key Features

- **Intuitive API**: Extension methods that read like natural language
- **Type-Safe**: Works with any type that implements `IComparable<T>`
- **Zero Dependencies**: Lightweight library with no external dependencies
- **Well-Tested**: Comprehensive test coverage to ensure reliability
- **Performance-Optimized**: Minimal overhead compared to manual comparison operations

## Available Extension Methods

### IsBetween

Determines if a value is **strictly between** two bounds (exclusive comparison):

```csharp
int value = 5;
bool result = value.IsBetween(1, 10); // true (5 > 1 AND 5 < 10)
```

### IsInRange

Determines if a value is **within the range** of two bounds (inclusive comparison):

```csharp
int value = 5;
bool result = value.IsInRange(5, 10); // true (5 >= 5 AND 5 <= 10)
```

## Use Cases

This library is particularly useful in scenarios such as:

- **Validation**: Checking if user input falls within acceptable ranges
- **Business Logic**: Implementing rules that depend on value ranges
- **Data Processing**: Filtering or categorizing data based on value ranges
- **Game Development**: Checking bounds for positions, scores, or other metrics
- **Financial Applications**: Validating amounts, dates, or other comparable values

## Supported Types

Any type that implements `IComparable<T>` can use these extension methods, including:

- Numeric types: `int`, `double`, `decimal`, `float`, etc.
- Date and time types: `DateTime`, `DateTimeOffset`, `TimeSpan`
- String comparisons
- Custom types that implement `IComparable<T>`

## License

This project is licensed under the Mozilla Public License 2.0. See the [LICENSE](../LICENSE) file for details.

## Contributing

We welcome contributions! Please see our [CONTRIBUTING.md](../CONTRIBUTING.md) guide for more information.
