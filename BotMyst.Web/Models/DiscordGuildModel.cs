using System;

using Newtonsoft.Json;

namespace BotMyst.Web.Models
{
    public class DiscordGuildModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("splash")]
        public string Splash { get; set; }

        [JsonProperty ("owner")]
        public bool Owner { get; set; }

        [JsonProperty("owner_id")]
        public string OwnerId { get; set; }

        [JsonProperty ("permissions")]
        public int Permissions { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("afk_channel_id")]
        public string AfkChannelId { get; set; }

        [JsonProperty("afk_timeout")]
        public int AfkTimeout { get; set; }

        [JsonProperty("embed_enabled")]
        public bool EmbedEnabled { get; set; }

        [JsonProperty("embed_channel_id")]
        public string EmbedChannelId { get; set; }

        [JsonProperty("verification_level")]
        public int VerificationLevel { get; set; }

        [JsonProperty("default_message_notifications")]
        public int DefaultMessageNotifications { get; set; }

        [JsonProperty("explicit_content_filter")]
        public int ExplicitContentFilter { get; set; }

        [JsonProperty("roles")]
        public DiscordRoleModel[] Roles { get; set; }

        [JsonProperty("emojis")]
        public DiscordEmojiModel [] Emojis { get; set; }

        [JsonProperty("features")]
        public string[] Features { get; set; }

        [JsonProperty("mfa_level")]
        public long MfaLevel { get; set; }

        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }

        [JsonProperty("widget_enabled")]
        public bool WidgetEnabled { get; set; }

        [JsonProperty("widget_channel_id")]
        public string WidgetChannelId { get; set; }

        [JsonProperty ("system_channel_id")]
        public string SystemChannelId { get; set; }

        [JsonProperty ("joined_at")]
        public DateTime JoinedAt { get; set; }

        [JsonProperty ("large")]
        public bool Large { get; set; }

        [JsonProperty("unavailable")]
        public bool Unavailable { get; set; }

        [JsonProperty ("member_count")]
        public int MemberCount { get; set; }

        [JsonProperty ("voice_states")]
        public DiscordVoiceStateModel [] VoiceStates { get; set; }

        [JsonProperty ("members")]
        public DiscordUserModel [] Members { get; set; }

        [JsonProperty ("channels")]
        public DiscordChannelModel [] Channels { get; set; }

        [JsonProperty ("presences")]
        public DiscordPresenceUpdateModel [] Presences { get; set; }

        public bool HasBotMyst { get; set; }
    }
}