﻿using System;
using System.Threading.Tasks;

namespace BotMyst.Bot
{
    public class Program
    {
        public static async Task Main ()
        {
            BotMyst bot = new BotMyst ();
            await bot.Start ();
        }
    }
}
