using System;

namespace BotMyst.Bot.Models
{
    public class CommandOptionSummaryAttribute : Attribute
    {
        public string Summary { get; private set; }

        public CommandOptionSummaryAttribute (string summary) => Summary = summary;
    }
}