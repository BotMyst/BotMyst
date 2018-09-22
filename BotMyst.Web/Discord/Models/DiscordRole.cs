using Newtonsoft.Json;

namespace BotMyst.Web.Discord.Models
{
    public class DiscordRole
    {
        [JsonProperty ("id")]
        public ulong Id { get; set; }

        [JsonProperty ("name")]
        public string Name { get; set; }

        [JsonProperty ("color")]
        public int Color { get; set; }

        [JsonProperty ("hoist")]
        public bool Hoist { get; set; }

        [JsonProperty ("position")]
        public int Position { get; set; }

        [JsonProperty ("permissions")]
        public int Permissions { get; set; }

        [JsonProperty ("managed")]
        public bool Managed { get; set; }
        
        [JsonProperty ("mentionable")]
        public bool Mentionable { get; set; }
    }
}