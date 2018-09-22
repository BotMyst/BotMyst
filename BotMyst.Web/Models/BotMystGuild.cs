using BotMyst.Web.Discord.Models;

namespace BotMyst.Web.Models
{
    public class BotMystGuild
    {
        public DiscordGuild Guild { get; set; }
        public bool HasBotMyst { get; set; }
    }
}