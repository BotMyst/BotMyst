using System;

namespace BotMyst.Bot
{
    public class CommandOptionSummaryAttribute : Attribute
    {
        public string Summary { get; private set; }

        public CommandOptionSummaryAttribute (string summary) => Summary = summary;
    }
}