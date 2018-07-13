using System.Reflection;
using System.Collections.Generic;

using BotMyst.Bot;

namespace BotMyst.Web.Models
{
    public class CommandSettingsModel
    {
        public DiscordGuildModel Guild { get; set; }
        public CommandDescriptionModel CommandDescription { get; set; }
        public CommandOptions CommandOptions { get; set; }
        public List<CommandOptionDescriptionModel> CommandOptionDescriptions { get; set; }
    }
}