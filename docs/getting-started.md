# Getting Started

This guide will help you quickly start using Wolfgang.Extensions.IComparable in your .NET projects.

## Prerequisites

- .NET 8.0 or later
- A C# project (any project type: console, web, library, etc.)

## Installation

### Using NuGet Package Manager

```bash
dotnet add package Wolfgang.Extensions.IComparable
```

### Using Package Manager Console (Visual Studio)

```powershell
Install-Package Wolfgang.Extensions.IComparable
```

### Using .csproj File

Add the following package reference to your `.csproj` file:

```xml
<ItemGroup>
  <PackageReference Include="Wolfgang.Extensions.IComparable" Version="1.*" />
</ItemGroup>
```

## Basic Usage

### 1. Add the Using Directive

Add the namespace to your C# file:

```csharp
using Wolfgang.Extensions.IComparable;
```

### 2. Use IsBetween Method

Check if a value is strictly between two bounds (exclusive):

```csharp
int temperature = 25;

if (temperature.IsBetween(0, 30))
{
    Console.WriteLine("Temperature is between 0 and 30 (exclusive)");
}
```

**Example Results:**
- `5.IsBetween(1, 10)` → `true` (5 is greater than 1 and less than 10)
- `1.IsBetween(1, 10)` → `false` (1 is not greater than 1)
- `10.IsBetween(1, 10)` → `false` (10 is not less than 10)

### 3. Use IsInRange Method

Check if a value is within a range (inclusive):

```csharp
int score = 85;

if (score.IsInRange(0, 100))
{
    Console.WriteLine("Score is within the valid range of 0-100");
}
```

**Example Results:**
- `5.IsInRange(1, 10)` → `true` (5 is between 1 and 10)
- `1.IsInRange(1, 10)` → `true` (1 equals the lower bound)
- `10.IsInRange(1, 10)` → `true` (10 equals the upper bound)

## Common Examples

### Validating User Input

```csharp
int age = GetUserAge();

if (age.IsInRange(0, 120))
{
    Console.WriteLine("Valid age entered");
}
else
{
    Console.WriteLine("Age must be between 0 and 120");
}
```

### Working with Decimals

```csharp
decimal price = 49.99m;

if (price.IsBetween(0m, 100m))
{
    Console.WriteLine("Price is in the medium range");
}
```

### Working with Dates

```csharp
DateTime checkDate = new DateTime(2024, 6, 15);
DateTime startDate = new DateTime(2024, 1, 1);
DateTime endDate = new DateTime(2024, 12, 31);

if (checkDate.IsInRange(startDate, endDate))
{
    Console.WriteLine("Date falls within 2024");
}
```

### Working with Strings

```csharp
string value = "M";

// String comparison is alphabetical
if (value.IsBetween("A", "Z"))
{
    Console.WriteLine("Value is between A and Z (exclusive)");
}
```

### Working with Custom Types

```csharp
public class Employee : IComparable<Employee>
{
    public string Name { get; set; }
    public decimal Salary { get; set; }
    
    public int CompareTo(Employee other)
    {
        return Salary.CompareTo(other.Salary);
    }
}

var employee = new Employee { Name = "John", Salary = 50000 };
var minSalary = new Employee { Salary = 30000 };
var maxSalary = new Employee { Salary = 100000 };

if (employee.IsInRange(minSalary, maxSalary))
{
    Console.WriteLine("Employee salary is within the expected range");
}
```

## Quick Reference

| Method | Comparison Type | Lower Bound | Upper Bound | Example |
|--------|----------------|-------------|-------------|---------|
| `IsBetween` | Exclusive | `>` | `<` | `5.IsBetween(1, 10)` → `true` |
| `IsInRange` | Inclusive | `>=` | `<=` | `1.IsInRange(1, 10)` → `true` |

## Next Steps

- Explore more [advanced usage examples](./readme.md)
- Learn about [setting up your development environment](./setup.md)
- Read the full [introduction](./introduction.md) to understand the library's design

## Troubleshooting

### Namespace Not Found

If you get a compile error that the namespace cannot be found:
1. Ensure the package is properly installed (`dotnet restore`)
2. Verify you're using .NET 8.0 or later
3. Check that your IDE has indexed the new package (restart if necessary)

### Method Not Available

If the extension methods don't appear:
1. Ensure you've added the `using Wolfgang.Extensions.IComparable;` directive
2. Verify your type implements `IComparable<T>`
3. Check that IntelliSense has refreshed (rebuild the solution)

## Getting Help

If you encounter any issues:
- Check the [GitHub Issues](https://github.com/Chris-Wolfgang/IComparable-Extensions/issues)
- Review the [examples](../examples/) in the repository
- Read the [API documentation](../README.md)
