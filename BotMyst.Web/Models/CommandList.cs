using System.Collections.Generic;

using BotMyst.Shared.Models;

namespace BotMyst.Web.Models
{
    public class CommandList
    {
        public List<ModuleDescription> ModuleDescriptions { get; set; }
        public BotMystGuild BotMystGuild { get; set; }
    }
}