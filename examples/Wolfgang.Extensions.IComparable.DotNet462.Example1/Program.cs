using System;

namespace Wolfgang.Extensions.IComparable.DotNet462.Example1
{
    internal static class Program
    {
        private static void Main()
        {
            Console.ForegroundColor = ConsoleColor.White;

            var value = 1;
            Console.WriteLine("   " + value + " IsBetween 1 and 10: " + value.IsBetween(1, 10));

            value = 2;
            Console.WriteLine("   " + value + " IsBetween 1 and 10: " + value.IsBetween(1, 10));

            value = 9;
            Console.WriteLine("   " + value + " IsBetween 1 and 10: " + value.IsBetween(1, 10));

            value = 10;
            Console.WriteLine("  " + value + " IsBetween 1 and 10: " + value.IsBetween(1, 10));

            Console.WriteLine();

            value = 1;
            Console.WriteLine("   " + value + " IsInRange 1 and 10: " + value.IsInRange(1, 10));

            value = 2;
            Console.WriteLine("   " + value + " IsInRange 1 and 10: " + value.IsInRange(1, 10));

            value = 9;
            Console.WriteLine("   " + value + " IsInRange 1 and 10: " + value.IsInRange(1, 10));

            value = 10;
            Console.WriteLine("  " + value + " IsInRange 1 and 10: " + value.IsInRange(1, 10));

            Console.WriteLine();

            Console.ResetColor();
        }
    }
}
