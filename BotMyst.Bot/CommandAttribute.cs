using System;

namespace BotMyst.Bot
{
    public class CommandAttribute : Attribute
    {
        public string Name { get; }
        public string Command { get; }
        public string Description { get; }

        public CommandAttribute (string name, string command, string description)
        {
            Name = name;
            Command = command;
            Description = description;
        }
    }
}