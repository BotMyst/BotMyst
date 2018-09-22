using Newtonsoft.Json;

namespace BotMyst.Web.Discord.Models
{
    public class DiscordOverwrite
    {
        [JsonProperty ("id")]
        public ulong Id { get; set; }
        
        [JsonProperty ("type")]
        public string Type { get; set; }
        
        [JsonProperty ("allow")]
        public int Allow { get; set; }
        
        [JsonProperty ("deny")]
        public int Deny { get; set; }
    }
}