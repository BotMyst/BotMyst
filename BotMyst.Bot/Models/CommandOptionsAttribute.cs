using System;

namespace BotMyst.Bot.Models
{
    public class CommandOptionsAttribute : Attribute
    {
        public Type CommandOptionsType { get; set; }

        public CommandOptionsAttribute (Type commandOptionsType)
        {
            CommandOptionsType = commandOptionsType;
        }
    }
}