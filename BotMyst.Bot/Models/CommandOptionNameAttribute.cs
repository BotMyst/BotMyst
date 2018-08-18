using System;

namespace BotMyst.Bot.Models
{
    public class CommandOptionNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public CommandOptionNameAttribute (string name) => Name = name;
    }
}