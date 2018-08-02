using System.ComponentModel.DataAnnotations;

namespace BotMyst.Bot
{
    public abstract class CommandOptions
    {
        [Key]
        public ulong GuildId { get; set; }
        public bool Enabled { get; set; } = true;

        [Toggle]
        [CommandOptionName ("DM")]
        [CommandOptionSummary ("Whether to send a direct message of the reply to the user calling the command")]
        public bool Dm { get; set; } = false;

        [Toggle]
        [CommandOptionName ("Delete the Invocation Message")]
        [CommandOptionSummary ("Whether to delete the invocation message of the user calling the command")]
        public bool DeleteInvocationMessage { get; set; } = false;

        [RoleList]
        [CommandOptionName ("Role whitelist")]
        [CommandOptionSummary ("List of roles that can use the command.")]
        public string RoleWhitelist { get; set; } = "";

        [RoleList]
        [CommandOptionName ("Role blacklist")]
        [CommandOptionSummary ("List of roles that can't use the command.")]
        public string RoleBlacklist { get; set; } = "";

        [ChannelList]
        [CommandOptionName ("Channel whitelist")]
        [CommandOptionSummary ("List of channel in which the command can be executed.")]
        public string ChannelWhitelist { get; set; } = "";

        [ChannelList]
        [CommandOptionName ("Channel blacklist")]
        [CommandOptionSummary ("List of channels in which the command can't be executed.")]
        public string ChannelBlacklist { get; set; } = "";
    }
}