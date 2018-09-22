using System;

using Newtonsoft.Json;

namespace BotMyst.Web.Discord.Models
{
    public class DiscordChannel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("guild_id")]
        public ulong GuildId { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("permission_overwrites")]
        public DiscordOverwrite [] PermissionOverwrites { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("nsfw")]
        public bool Nsfw { get; set; }

        [JsonProperty("last_message_id")]
        public ulong LastMessageId { get; set; }

        [JsonProperty ("bitrate")]
        public int Bitrate { get; set; }

        [JsonProperty ("user_limit")]
        public int UserLimit { get; set; }

        [JsonProperty ("recipients")]
        public DiscordUser [] Reciepients { get; set; }

        [JsonProperty ("icon")]
        public string Icon { get; set; }

        [JsonProperty ("owner_id")]
        public ulong OwnerId { get; set; }

        [JsonProperty ("application_id")]
        public ulong ApplicationId { get; set; }

        [JsonProperty("parent_id")]
        public ulong ParentId { get; set; }

        [JsonProperty ("last_pin_timestamp")]
        public DateTime LastPinTimestamp { get; set; }
    }
}