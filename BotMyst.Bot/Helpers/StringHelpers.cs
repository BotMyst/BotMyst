using System;
using System.Collections.Generic;

namespace BotMyst.Bot.Helpers
{
    public static class StringHelpers
    {
        public static string UppercaseFirst (this string s)
        {
            if (string.IsNullOrEmpty (s))
            {
                return string.Empty;
            }
            return char.ToUpper (s [0]) + s.Substring (1);
        }

        public static IEnumerable<string> SplitEveryNth (this string s, int n)
        {
            if (s.Length == 0)
            {
                throw new ArgumentNullException ("s");
            }
            if (n <= 0)
            {
                throw new ArgumentNullException ("N has to be positive.", "n");
            }

            for (int i = 0; i < s.Length; i += n)
            {
                yield return s.Substring (i, Math.Min (n, s.Length - i));
            }
        }

        public static string NormalizeWhitespaces (this string input)
        {
            if (string.IsNullOrEmpty (input))
                return string.Empty;

            int current = 0;
            char[] output = new char[input.Length];
            bool skipped = false;

            foreach (char c in input.ToCharArray ())
            {
                if (char.IsWhiteSpace(c))
                {
                    if (!skipped)
                    {
                        if (current > 0)
                            output [current++] = ' ';

                        skipped = true;
                    }
                }
                else
                {
                    skipped = false;
                    output [current++] = c;
                }
            }

            return new string (output, 0, current);
        }

        public static ulong GetUserId (this string input)
        {
            ulong.TryParse (input.Substring (2, 18), out ulong result);
            return result;
        }
    }
}