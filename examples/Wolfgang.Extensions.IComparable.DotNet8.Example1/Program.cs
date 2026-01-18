// See https://aka.ms/new-console-template for more information

namespace Wolfgang.Extensions.IComparable.DotNet8.Example1;

internal static class Program
{
    public static void Main()
    {
        
        Console.ForegroundColor = ConsoleColor.White;

        var value = 1;
        Console.WriteLine($"   {value} IsBetween 1 and 10: {value.IsBetween(1, 10)}");

        value = 2;
        Console.WriteLine($"   {value} IsBetween 1 and 10: {value.IsBetween(1, 10)}");

        value = 9;
        Console.WriteLine($"   {value} IsBetween 1 and 10: {value.IsBetween(1, 10)}");

        value = 10;
        Console.WriteLine($"  {value} IsBetween 1 and 10: {value.IsBetween(1, 10)}");

        Console.WriteLine("\n");

        value = 1;
        Console.WriteLine($"   {value} IsInRange 1 and 10: {value.IsInRange(1, 10)}");

        value = 2;
        Console.WriteLine($"   {value} IsInRange 1 and 10: {value.IsInRange(1, 10)}");

        value = 9;
        Console.WriteLine($"   {value} IsInRange 1 and 10: {value.IsInRange(1, 10)}");

        value = 10;
        Console.WriteLine($"  {value} IsInRange 1 and 10: {value.IsInRange(1, 10)}");

        Console.WriteLine("\n");

        Console.ResetColor();
    }
}