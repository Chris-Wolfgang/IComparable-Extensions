using System;

namespace Wolfgang.Extensions.IComparable.DotNet462.Example1
{
    internal static class Program
    {
        private static void Main()
        {
            Console.ForegroundColor = ConsoleColor.White;

            int[] values = { 1, 2, 9, 10 };

            foreach (var value in values)
            {
                Console.WriteLine($"   {value,2} IsBetween 1 and 10: {value.IsBetween(1, 10)}");
            }

            Console.WriteLine();

            foreach (var value in values)
            {
                Console.WriteLine($"   {value,2} IsInRange 1 and 10: {value.IsInRange(1, 10)}");
            }

            Console.WriteLine();

            Console.ResetColor();
        }
    }
}
