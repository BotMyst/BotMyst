using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

using BotMyst.Bot.Models;
using BotMyst.Bot.Helpers;
using BotMyst.Bot.Options.Utility;

namespace BotMyst.Bot.Commands.Utility
{
    public partial class Utility : Module
    {
        [Name ("User Info")]
        [Command ("userinfo")]
        [Summary ("Gets information about the specified user")]
        [CommandOptions (typeof (UserInfoOptions))]
        public async Task UserInfo ([Remainder] IGuildUser user)
        {
            var options = GetOptions<UserInfoOptions> ();

            EmbedBuilder emb = new EmbedBuilder();

            string userRoles = DiscordHelpers.GetListOfUsersRoles (user);

            // Find user's highest role so the embed will be coloured with the role's colour

            IRole highestRole = DiscordHelpers.GetUsersHigherstRole (user);

            if (highestRole != null)
                emb.Color = highestRole.Color;

            // Display if the user is a bot or a webhook
            EmbedAuthorBuilder author = new EmbedAuthorBuilder ();
            author.Name = user.Username;
            if (user.IsBot)
                author.Name += " (Bot)";
            else if (user.IsWebhook)
                author.Name += " (Webhook)";

            emb.Author = author;

            // If the user has a default avatar
            if (string.IsNullOrEmpty (user.AvatarId))
                emb.ThumbnailUrl = $"https://discordapp.com/assets/dd4dbc0016779df1378e7812eabaa04d.png";
            else
                emb.ThumbnailUrl = $"https://cdn.discordapp.com/avatars/{user.Id}/{user.AvatarId}.png";

            EmbedFooterBuilder footer = new EmbedFooterBuilder();
            footer.Text = $"User info requested by {Context.User.Username}";
            // If the user has a default avatar
            if (string.IsNullOrEmpty (Context.User.AvatarId))
                footer.IconUrl = $"https://discordapp.com/assets/dd4dbc0016779df1378e7812eabaa04d.png";
            else
                footer.IconUrl = $"https://cdn.discordapp.com/avatars/{Context.User.Id}/{Context.User.AvatarId}.png";
            emb.Footer = footer;

            emb.Description = $"User information for {user.Username}#{user.Discriminator} | {user.Id}";

            emb.AddField("Created account at", user.CreatedAt.ToString("dd MMM yyyy, HH:mm"));

            emb.AddField("Joined server at", ((DateTimeOffset)user.JoinedAt).ToString("dd MMM yyyy, HH:mm"));

            // Display the list of all of user's roles
            if (string.IsNullOrEmpty(userRoles) == false)
                emb.AddField("Role(s)", userRoles);

            // Display the list of all of user's permissions
            string userPermissions = GetUserPermissions (user);

            if (string.IsNullOrEmpty (userPermissions) == false)
                emb.AddField ("Permissions", userPermissions);

            emb.AddField("Online status", user.Status == UserStatus.DoNotDisturb ? "Do Not Disturb" : user.Status.ToString());

            if (user.Game.HasValue)
            {
                if (user.Game.Value.StreamType == StreamType.Twitch)
                {
                    emb.AddField("Streaming", user.Game.Value.StreamUrl);
                }
                else
                {
                    emb.AddField("Playing", user.Game.Value.Name);
                }
            }

            await SendMessage (options, "", false, emb.Build());
        }

        /// <summary>
        /// Get a list of user's permissions in a nicely formatted string.
        /// </summary>
        private string GetUserPermissions (IGuildUser user)
        {
            string permissions = "";

            if (Context.Guild.OwnerId == user.Id)
            {
                permissions += "Owner";
                return permissions;
            }

            if (user.GuildPermissions.Administrator)
            {
                permissions += "Administrator";
                return permissions;
            }

            if (user.GuildPermissions.BanMembers)
                permissions += "Ban Memebers, ";
        
            if (user.GuildPermissions.DeafenMembers)
                permissions += "Deafen Members, ";

            if (user.GuildPermissions.KickMembers)
                permissions += "Kick Members, ";

            if (user.GuildPermissions.ManageChannels)
                permissions += "Manage Channels, ";

            if (user.GuildPermissions.ManageEmojis)
                permissions += "Manage Emojis, ";

            if (user.GuildPermissions.ManageGuild)
                permissions += "Manage Guild, ";

            if (user.GuildPermissions.ManageMessages)
                permissions += "Manage Messages, ";

            if (user.GuildPermissions.ManageNicknames)
                permissions += "Manage Nicknames, ";

            if (user.GuildPermissions.ManageRoles)
                permissions += "Manage Roles, ";

            if (user.GuildPermissions.ManageWebhooks)
                permissions += "Manage Webhooks, ";

            if (user.GuildPermissions.MentionEveryone)
                permissions += "Mention Everyone, ";

            if (user.GuildPermissions.MoveMembers)
                permissions += "Move Members, ";

            if (user.GuildPermissions.MuteMembers)
                permissions += "Mute Members, ";

            return permissions.Remove (permissions.Length - 2);
        }
    }
}