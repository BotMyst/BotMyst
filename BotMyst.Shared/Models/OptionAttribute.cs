using System;

namespace BotMyst.Shared.Models
{
    public class OptionAttribute : Attribute
    {
        public string Summary { get; private set; }
        public OptionTypes OptionType { get; private set; }

        public OptionAttribute (string summary, OptionTypes optionType)
        {
            Summary = summary;
            OptionType = optionType;
        }
    }
}