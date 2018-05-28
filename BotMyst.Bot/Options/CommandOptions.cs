namespace BotMyst.Bot.Options
{
    public class CommandOptions
    {
        public bool Enabled { get; set; } = true;
        public bool Dm { get; set; } = false;
        public string IncludeRoles { get; set; }
        public string ExcludeRoles { get; set; }
        public string IncludeChannels { get; set; }
        public string ExcludeChannels { get; set; }
        public bool DeleteInvocationMessage { get; set; } = false;
    }
}