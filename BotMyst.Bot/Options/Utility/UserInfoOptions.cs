namespace BotMyst.Bot.Options
{
    public class UserInfoOptions : CommandOptions
    {
        public bool ShowCreatedAt { get; set; } = true;
        public bool ShowJoinedAt { get; set; } = true;
        public bool ShowRoles { get; set; } = true;
        public bool ShowOnlineStatus { get; set; } = true;
        public bool ShowGame { get; set; } = true;
    }
}