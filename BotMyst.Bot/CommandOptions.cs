using System.ComponentModel.DataAnnotations;

namespace BotMyst.Bot
{
    public abstract class CommandOptions
    {
        [Key]
        public ulong GuildId { get; set; }
        public bool Enabled { get; set; }
        public bool Dm { get; set; }
        public bool DeleteInvocationMessage { get; set; }
        public string RoleWhitelist { get; set; }
        public string RoleBlacklist { get; set; }
        public string ChannelWhitelist { get; set; }
        public string ChannelBlacklist { get; set; }
    }
}