using System.ComponentModel.DataAnnotations;

namespace BotMyst.Bot
{
    public abstract class CommandOptions
    {
        [Key]
        public ulong GuildId { get; set; }
        public bool Enabled { get; set; } = true;
        public bool Dm { get; set; } = false;
        public bool DeleteInvocationMessage { get; set; } = false;
        public string RoleWhitelist { get; set; } = "";
        public string RoleBlacklist { get; set; } = "";
        public string ChannelWhitelist { get; set; } = "";
        public string ChannelBlacklist { get; set; } = "";
    }
}