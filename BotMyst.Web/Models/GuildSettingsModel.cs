using System.Collections.Generic;

namespace BotMyst.Web.Models
{
    public class GuildSettingsModel
    {
        public List<ModuleDescriptionModel> Modules { get; set; }
        public DiscordGuildModel Guild { get; set; }
    }
}