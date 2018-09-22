using Newtonsoft.Json;

namespace BotMyst.Web.Discord.Models
{
    public class DiscordEmoji
    {
        [JsonProperty ("id")]
        public ulong Id { get; set; }

        [JsonProperty ("name")]
        public string Name { get; set; }

        [JsonProperty ("roles")]
        public DiscordRole [] Roles { get; set; }

        [JsonProperty ("user")]
        public DiscordUser [] User { get; set; }

        [JsonProperty ("require_colons")]
        public bool RequireColons { get; set; }

        [JsonProperty ("managed")]
        public bool Managed { get; set; }

        [JsonProperty ("animated")]
        public bool Animated { get; set; }
    }
}