using Newtonsoft.Json;

namespace BotMyst.Web.Models
{
    public class DiscordPresenceUpdateModel
    {
        [JsonProperty ("user")]
        public DiscordUserModel User { get; set; }
        [JsonProperty ("roles")]
        public string [] Roles { get; set; }
        // TODO: Make the activity model. It's pretty complex and I hate making these models :/
        // [JsonProperty ("game")]
        // public DiscordActivityModel Game { get; set; }
        [JsonProperty ("guild_id")]
        public string GuildId { get; set; }
        [JsonProperty ("status")]
        public string Status { get; set; }
    }
}