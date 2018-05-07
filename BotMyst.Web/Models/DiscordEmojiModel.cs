using Newtonsoft.Json;

namespace BotMyst.Web.Models
{
    public class DiscordEmojiModel
    {
        [JsonProperty ("id")]
        public string Id { get; set; }
        [JsonProperty ("name")]
        public string Name { get; set; }
        [JsonProperty ("roles")]
        public DiscordRoleModel [] Roles { get; set; }
        [JsonProperty ("user")]
        public DiscordUserModel [] User { get; set; }
        [JsonProperty ("require_colons")]
        public bool RequireColons { get; set; }
        [JsonProperty ("managed")]
        public bool Managed { get; set; }
        [JsonProperty ("animated")]
        public bool Animated { get; set; }
    }
}