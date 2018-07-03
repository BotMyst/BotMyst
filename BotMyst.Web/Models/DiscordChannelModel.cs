using System;

using Newtonsoft.Json;

namespace BotMyst.Web.Models
{
    public class DiscordChannelModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("guild_id")]
        public string GuildId { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("permission_overwrites")]
        public DiscordOverwriteModel [] PermissionOverwrites { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("nsfw")]
        public bool Nsfw { get; set; }

        [JsonProperty("last_message_id")]
        public string LastMessageId { get; set; }

        [JsonProperty ("bitrate")]
        public int Bitrate { get; set; }

        [JsonProperty ("user_limit")]
        public int UserLimit { get; set; }

        [JsonProperty ("recipients")]
        public DiscordUserModel [] Reciepients { get; set; }

        [JsonProperty ("icon")]
        public string Icon { get; set; }

        [JsonProperty ("owner_id")]
        public string OwnerId { get; set; }

        [JsonProperty ("application_id")]
        public string ApplicationId { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty ("last_pin_timestamp")]
        public DateTime LastPinTimestamp { get; set; }
    }
}