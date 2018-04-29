using System;
using System.Collections.Generic;

namespace BotMyst.Helpers
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
    }
}