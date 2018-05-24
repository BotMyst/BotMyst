using System;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord;
using Discord.Commands;

namespace BotMyst.Bot.Modules
{
    public class Utility : ModuleBase
    {
        [Command ("lmgtfy")]
        [Alias ("google")]
        [Summary ("Generates a lmgtfy link for people too lazy / stupid to google on their own")]
        public async Task Lmgtfy ([Remainder] string searchString)
        {
            string url = $"https://lmgtfy.com/?q={HttpUtility.UrlEncode (searchString)}";

            EmbedBuilder embed = new EmbedBuilder ();
            embed.Title = "LMGTFY (Google)";
            embed.Description = url;

            await ReplyAsync ("", false, embed.Build ());
        }

        [Command ("userinfo")]
        [Alias ("user")]
        [Summary ("Retrieves information about a specified user.")]
        private async Task UserInfo ([Remainder] IGuildUser user)
        {
            EmbedBuilder emb = new EmbedBuilder ();

            // Find user's highest role so the embed will be coloured with the role's colour

            IReadOnlyCollection<ulong> roleIds = user.RoleIds;
            
            string userRoles = "";

            IRole highestRole = null;
            foreach (ulong id in roleIds)
            {
                IRole role = Context.Guild.GetRole (id);
                if (role.Name == "@everyone")
                    continue;

                userRoles += $"{role.Name}, ";

                if (highestRole == null)
                {
                    highestRole = role;
                }
                else if (role.Position > highestRole.Position)
                {
                    highestRole = role;
                }
            }

            if (string.IsNullOrEmpty (userRoles) == false)
                userRoles = userRoles.Remove (userRoles.Length - 2, 2);

            if (highestRole != null)
                emb.Color = highestRole.Color;
                
            EmbedAuthorBuilder author = new EmbedAuthorBuilder ();
            author.Name = user.Username;
            if (user.IsBot)
                author.Name += " (Bot)";
            else if (user.IsWebhook)
                author.Name += " (Webhook)";
            
            emb.Author = author;

            emb.ThumbnailUrl = $"https://cdn.discordapp.com/avatars/{user.Id}/{user.AvatarId}.png";
            
            EmbedFooterBuilder footer = new EmbedFooterBuilder ();
            footer.Text = $"User info requested by {Context.User.Username}";
            footer.IconUrl = $"https://cdn.discordapp.com/avatars/{Context.User.Id}/{Context.User.AvatarId}.png";
            emb.Footer = footer;
            
            emb.Description = $"User information for {user.Username}#{user.Discriminator}";

            emb.AddField ("Created account at", user.CreatedAt.ToString ("dd MMM yyyy, HH:mm"));

            emb.AddField ("Joined server at", ((DateTimeOffset) user.JoinedAt).ToString ("dd MMM yyyy, HH:mm"));

            if (string.IsNullOrEmpty (userRoles) == false)
                emb.AddField ("Role(s)", userRoles);

            emb.AddField ("Online status", user.Status == UserStatus.DoNotDisturb ? "Do Not Disturb" : user.Status.ToString ());

            if (user.Game.HasValue)
            {
                if (user.Game.Value.StreamType == StreamType.Twitch)
                {
                    emb.AddField ("Streaming", user.Game.Value.StreamUrl);
                }
                else
                {
                    emb.AddField ("Playing", user.Game.Value.Name);
                }
            }

            await ReplyAsync ("", false, emb.Build ());
        }
    }
}