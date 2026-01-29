# Wolfgang.Extensions.IComparable

A powerful and intuitive .NET library that extends the `IComparable<T>` interface with convenient range-checking methods.

## What Does It Do?

This library provides two essential extension methods that make range comparisons simple and readable:

### IsBetween (Exclusive Range)

Returns `true` when a value is **strictly greater than** the lower bound AND **strictly less than** the upper bound.

```csharp
5.IsBetween(1, 10)  // true  (5 > 1 AND 5 < 10)
1.IsBetween(1, 10)  // false (1 is not > 1)
10.IsBetween(1, 10) // false (10 is not < 10)
```

### IsInRange (Inclusive Range)

Returns `true` when a value is **greater than or equal to** the lower bound AND **less than or equal to** the upper bound.

```csharp
5.IsInRange(1, 10)  // true (5 >= 1 AND 5 <= 10)
1.IsInRange(1, 10)  // true (1 >= 1 AND 1 <= 10)
10.IsInRange(1, 10) // true (10 >= 1 AND 10 <= 10)
```

## Installation

Install via NuGet:

```bash
dotnet add package Wolfgang.Extensions.IComparable
```

## Quick Start

```csharp
using Wolfgang.Extensions.IComparable;

// Check if a number is in a valid range
int temperature = 22;
if (temperature.IsInRange(18, 26))
{
    Console.WriteLine("Temperature is comfortable");
}

// Check if a date is between two dates (exclusive)
DateTime today = DateTime.Now;
DateTime start = new DateTime(2024, 1, 1);
DateTime end = new DateTime(2024, 12, 31);

if (today.IsBetween(start, end))
{
    Console.WriteLine("Date is within 2024 (exclusive of boundaries)");
}
```

## Detailed Examples

### Numeric Validation

```csharp
// Age validation
int age = 25;
bool isAdult = age.IsInRange(18, 120);

// Percentage validation
decimal percentage = 85.5m;
bool isValidPercentage = percentage.IsInRange(0m, 100m);

// Temperature monitoring
double temp = 98.6;
bool hasFeaver = temp.IsBetween(98.6, 103.0); // false (not > 98.6)
```

### String Comparisons

```csharp
string grade = "B";

// Check if grade is between A and C (alphabetically)
bool isPassing = grade.IsInRange("A", "D"); // true

// Check for specific range (exclusive)
bool isBetweenAandC = grade.IsBetween("A", "C"); // true (B is after A and before C)
```

### DateTime Operations

```csharp
DateTime appointmentTime = new DateTime(2024, 6, 15, 14, 30, 0);
DateTime officeOpen = new DateTime(2024, 6, 15, 9, 0, 0);
DateTime officeClosed = new DateTime(2024, 6, 15, 17, 0, 0);

// Check if appointment is during business hours
bool isDuringBusinessHours = appointmentTime.IsInRange(officeOpen, officeClosed);

// Check if time is strictly between opening and closing
bool isNotAtBoundary = appointmentTime.IsBetween(officeOpen, officeClosed);
```

### Custom Types

Any type implementing `IComparable<T>` works automatically:

```csharp
public class Version : IComparable<Version>
{
    public int Major { get; set; }
    public int Minor { get; set; }
    
    public int CompareTo(Version other)
    {
        int result = Major.CompareTo(other.Major);
        return result != 0 ? result : Minor.CompareTo(other.Minor);
    }
}

var currentVersion = new Version { Major = 2, Minor = 5 };
var minVersion = new Version { Major = 2, Minor = 0 };
var maxVersion = new Version { Major = 3, Minor = 0 };

bool isSupported = currentVersion.IsInRange(minVersion, maxVersion);
```

## Real-World Use Cases

### Input Validation

```csharp
public bool ValidateInput(int value)
{
    const int MIN_VALUE = 1;
    const int MAX_VALUE = 100;
    
    return value.IsInRange(MIN_VALUE, MAX_VALUE);
}
```

### Business Hours Checker

```csharp
public bool IsWithinBusinessHours(DateTime timestamp)
{
    var openTime = new TimeSpan(9, 0, 0);   // 9:00 AM
    var closeTime = new TimeSpan(17, 0, 0); // 5:00 PM
    
    return timestamp.TimeOfDay.IsInRange(openTime, closeTime);
}
```

### Price Range Filtering

```csharp
public List<Product> FilterByPriceRange(List<Product> products, decimal min, decimal max)
{
    return products.Where(p => p.Price.IsInRange(min, max)).ToList();
}
```

### Game Development - Boundary Checking

```csharp
public class GameCharacter
{
    public bool IsInPlayArea(float x, float y)
    {
        const float MIN_X = 0f;
        const float MAX_X = 100f;
        const float MIN_Y = 0f;
        const float MAX_Y = 100f;
        
        return x.IsInRange(MIN_X, MAX_X) && y.IsInRange(MIN_Y, MAX_Y);
    }
}
```

## API Reference

### IsBetween&lt;T&gt;

```csharp
public static bool IsBetween<T>(this T value, T lowerBound, T upperBound) 
    where T : IComparable<T>
```

**Parameters:**
- `value`: The value to compare
- `lowerBound`: The lower end of the range (exclusive)
- `upperBound`: The upper end of the range (exclusive)

**Returns:** `true` if `value > lowerBound AND value < upperBound`, otherwise `false`

### IsInRange&lt;T&gt;

```csharp
public static bool IsInRange<T>(this T value, T lowerBound, T upperBound) 
    where T : IComparable<T>
```

**Parameters:**
- `value`: The value to compare
- `lowerBound`: The lower end of the range (inclusive)
- `upperBound`: The upper end of the range (inclusive)

**Returns:** `true` if `value >= lowerBound AND value <= upperBound`, otherwise `false`

## Performance

Both methods are lightweight wrappers around the `CompareTo` method with minimal overhead:

- **IsBetween**: 2 comparison operations
- **IsInRange**: 2 comparison operations

Performance is equivalent to writing the comparisons manually but with significantly improved readability.

## Requirements

- .NET 8.0 or later
- Any C# project type (console, web, library, etc.)

## Documentation

- [Introduction](./introduction.md) - Learn about the library and its benefits
- [Getting Started](./getting-started.md) - Installation and basic usage
- [Setup Guide](./setup.md) - Development environment setup

## Contributing

Contributions are welcome! Please read our [Contributing Guidelines](../CONTRIBUTING.md) before submitting pull requests.

## Code of Conduct

This project follows the [Contributor Covenant Code of Conduct](../CODE_OF_CONDUCT.md). Please be respectful and constructive in all interactions.

## License

This project is licensed under the [Mozilla Public License 2.0](../LICENSE).

## Support

- Report bugs or request features via [GitHub Issues](https://github.com/Chris-Wolfgang/IComparable-Extensions/issues)
- View examples in the [examples folder](../examples/)
- Check out the full source code on [GitHub](https://github.com/Chris-Wolfgang/IComparable-Extensions)

## Acknowledgments

Built with ❤️ by the Wolfgang Extensions team.
