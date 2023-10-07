using System;

namespace PrimeNumber.Shared
{
    public static class ConsoleExt
    {
        public static void WriteInColor(ConsoleColor color, string text)
        {
            var initialColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = initialColor;
        }

        public static void WriteLineInColor(ConsoleColor color, string text)
        {
            WriteLineInColor((color, text));
        }

        public static void WriteLineInColor(params (ConsoleColor color, string text)[] stringsAndColors)
        {
            foreach (var pair in stringsAndColors)
            {
                WriteInColor(pair.Item1, pair.Item2);
            }
            Console.WriteLine();
        }
    }
}
