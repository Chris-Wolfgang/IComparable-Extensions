# Getting Started

This guide will help you quickly get up and running with Wolfgang.Extensions.IComparable.

## Prerequisites

- .NET 8.0 SDK or later (for development; the library targets .NET Framework 4.6.2+, .NET Standard 2.0, and .NET 8.0+)

## Installation

### Via NuGet Package Manager

```bash
dotnet add package Wolfgang.Extensions.IComparable
```

### Via Package Manager Console

```powershell
Install-Package Wolfgang.Extensions.IComparable
```

## Quick Start

```csharp
using Wolfgang.Extensions.IComparable;

int temperature = 25;

// IsBetween — strict (exclusive) bounds
if (temperature.IsBetween(0, 30))
{
    Console.WriteLine("Temperature is strictly between 0 and 30");
}

// IsInRange — inclusive bounds
int score = 85;
if (score.IsInRange(0, 100))
{
    Console.WriteLine("Score is within 0-100 (inclusive)");
}
```

## Examples

### Validating user input

```csharp
int age = GetUserAge();

if (!age.IsInRange(0, 120))
{
    throw new ArgumentOutOfRangeException(nameof(age), "Age must be between 0 and 120");
}
```

### Working with decimals

```csharp
decimal price = 49.99m;

if (price.IsBetween(0m, 100m))
{
    Console.WriteLine("Price is in the medium range");
}
```

### Working with dates

```csharp
var checkDate = new DateTime(2026, 6, 15);
var startDate = new DateTime(2026, 1, 1);
var endDate   = new DateTime(2026, 12, 31);

if (checkDate.IsInRange(startDate, endDate))
{
    Console.WriteLine("Date falls within 2026");
}
```

### Working with strings

```csharp
string value = "M";

// String comparison is ordinal/cultural per the IComparable<string> contract
if (value.IsBetween("A", "Z"))
{
    Console.WriteLine("Value is between A and Z (exclusive)");
}
```

### Working with custom types

```csharp
public sealed class Employee : IComparable<Employee>
{
    public string Name { get; init; }
    public decimal Salary { get; init; }

    public int CompareTo(Employee other) =>
        other is null ? 1 : Salary.CompareTo(other.Salary);
}

var employee  = new Employee { Name = "John", Salary = 50_000m };
var minSalary = new Employee { Salary = 30_000m };
var maxSalary = new Employee { Salary = 100_000m };

if (employee.IsInRange(minSalary, maxSalary))
{
    Console.WriteLine("Salary is within the expected range");
}
```

## Quick Reference

| Method     | Comparison | Lower | Upper | Example                          |
|------------|------------|-------|-------|----------------------------------|
| `IsBetween` | Exclusive  | `>`   | `<`   | `5.IsBetween(1, 10)` → `true`   |
| `IsInRange` | Inclusive  | `>=`  | `<=`  | `1.IsInRange(1, 10)` → `true`   |

## Next Steps

- Explore the [API Reference](https://chris-wolfgang.github.io/IComparable-Extensions/versions/latest/api/Wolfgang.Extensions.IComparable.html) for detailed documentation
- Read the [Introduction](introduction.md) to learn more about Wolfgang.Extensions.IComparable
- Check out the source on the [GitHub repository](https://github.com/Chris-Wolfgang/IComparable-Extensions)

## Common Issues

### Method not available on IntelliSense

Ensure the `using Wolfgang.Extensions.IComparable;` directive is present and that the type you're calling the extension on implements `IComparable<T>` (most BCL types do).

### Null arguments

`IsBetween` / `IsInRange` throw `ArgumentNullException` when the value itself is null. Null bounds are passed through to `T.CompareTo`, which for `string` returns `+1` against null — so the inclusive lower-bound check passes against a null lower bound while the upper-bound check fails against a null upper bound.

## Additional Resources

- [GitHub Repository](https://github.com/Chris-Wolfgang/IComparable-Extensions)
- [Contributing Guidelines](https://github.com/Chris-Wolfgang/IComparable-Extensions/blob/main/CONTRIBUTING.md)
- [Report an Issue](https://github.com/Chris-Wolfgang/IComparable-Extensions/issues)
