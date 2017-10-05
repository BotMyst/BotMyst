using System;

namespace BotMyst
{
    public class Program
    {
        private static void Main ()
        {
            BotMyst bot = new BotMyst ();
            bot.Start ().GetAwaiter ().GetResult ();
        }
    }
}
