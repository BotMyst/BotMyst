using System.Collections.Generic;

using BotMyst.Shared.Models;
using BotMyst.Shared.Models.CommandOptions;

namespace BotMyst.Web.Models
{
    public class CommandOptionsModel
    {
        public BotMystGuild BotMystGuild { get; set; }
        public CommandDescription CommandDescription { get; set; }
        public CommandOptions CommandOptions { get; set; }
        public List<OptionDescription> Options { get; set; }
    }
}