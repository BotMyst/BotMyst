namespace BotMyst.Bot.Options
{
    public class CommandOptions
    {
        public bool Enabled { get; set; } = true;
        public bool Dm { get; set; } = false;
        public ulong [] IncludeRoles { get; set; }
        public ulong [] ExcludeRoles { get; set; }
        public ulong [] IncludeChannels { get; set; }
        public ulong [] ExcludeChannels { get; set; }
        public bool DeleteInvocationMessage { get; set; } = false;
    }
}