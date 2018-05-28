using System.ComponentModel.DataAnnotations.Schema;

namespace BotMyst.Bot.Options
{
    public class UserInfoOptions : CommandOptions
    {
        [DatabaseGenerated (DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
        public bool ShowCreatedAt { get; set; } = true;
        public bool ShowJoinedAt { get; set; } = true;
        public bool ShowRoles { get; set; } = true;
        public bool ShowOnlineStatus { get; set; } = true;
        public bool ShowGame { get; set; } = true;
    }
}