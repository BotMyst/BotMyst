using System;
using System.Collections.Generic;

namespace BotMyst.Bot.Helpers
{
    public static class StringHelpers
    {
        /// <summary>
        /// Make the string's first letter uppercase.
        /// </summary>
        public static string UppercaseFirst (this string s)
        {
            if (string.IsNullOrEmpty (s))
                return string.Empty;
            return char.ToUpper (s [0]) + s.Substring (1);
        }

        /// <summary>
        /// Splits the string every nth character into a new list of separated strings.
        /// </summary>
        /// <returns>Returns the list of separated strings.</returns>
        public static IEnumerable<string> SplitEveryNth (this string s, int n)
        {
            if (s.Length == 0)
                throw new ArgumentNullException ("s");
            if (n <= 0)
                throw new ArgumentNullException ("N has to be positive.", "n");

            for (int i = 0; i < s.Length; i += n)
                yield return s.Substring (i, Math.Min (n, s.Length - i));
        }

        /// <summary>
        /// Normalizes whitespaces, removes duplicated sequential whitespaces.
        /// </summary>
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
    }
}