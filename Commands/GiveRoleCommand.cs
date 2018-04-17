using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Net;
using Discord.Commands;
using Discord.WebSocket;

namespace BotMyst.Commands
{
    [Summary ("Users can assing themselves roles. You can specify which roles should the users be able to assign.")]
    public class GiveRoleCommand : ModuleBase
    {
        [Command ("giverole"), Summary ("Gives you the specified role (if exists)."), Alias ("gimme")]
        [RequireBotPermission (GuildPermission.ManageRoles)]
        [RequireContext (ContextType.Guild)]
        public async Task GiveRole ([Remainder] IRole role)
        {
            IUser user = Context.User;
            SocketGuildUser guildUser = user as SocketGuildUser;

            if (guildUser.Roles.Contains (role))
            {
                EmbedBuilder goldB = new EmbedBuilder ();
                goldB.WithColor (Color.Gold);
                goldB.WithDescription ($"{user.Mention}, you already have the **{role.Name}** role.");
                await ReplyAsync (string.Empty, false, goldB);
                return;
            }

            try
            {
                await guildUser.AddRoleAsync (role);
            }
            catch (HttpException e)
            {
                if (e.HttpCode == HttpStatusCode.Forbidden)
                {
                    EmbedBuilder errorB = new EmbedBuilder ();
                    errorB.WithColor (Color.Red);
                    errorB.WithTitle ("Error");
                    errorB.WithDescription ("I cannot do that.");
                    await ReplyAsync (string.Empty, false, errorB);
                }
                return;
            }
            
            // Removes all of the user's roles that are below the "BotMyst" role,
            // because you are only allowed to have one color at a time.
            foreach (IRole r in guildUser.Roles)
            {
                if (r.Name != "@everyone" && r.Position < Context.Guild.Roles.FirstOrDefault (x => x.Name == "BotMyst").Position)
                {
                    await guildUser.RemoveRoleAsync (r);
                }
            }
            
            EmbedBuilder eb = new EmbedBuilder ();
            eb.WithColor (Color.Green);
            eb.WithTitle ("Success");
            eb.WithDescription ($"{user.Mention}, you have been given the **{role.Name}** role.");
            await ReplyAsync (string.Empty, false, eb);
        }

        [Command ("removerole"), Summary ("Removes the specified role (if you have it).")]
        [RequireBotPermission (GuildPermission.ManageRoles)]
        [RequireContext (ContextType.Guild)]
        public async Task RemoveRole ([Remainder] IRole role)
        {
            IUser user = Context.User;
            SocketGuildUser guildUser = user as SocketGuildUser;

            if (guildUser.Roles.Contains (role) == false)
            {
                EmbedBuilder goldB = new EmbedBuilder ();
                goldB.WithColor (Color.Gold);
                goldB.WithDescription ($"{user.Mention}, you don't have the **{role.Name}** role.");
                await ReplyAsync (string.Empty, false, goldB);
                return;
            }

            try
            {
                await guildUser.RemoveRoleAsync (role);
            }
            catch (HttpException e)
            {
                if (e.HttpCode == HttpStatusCode.Forbidden)
                {
                    EmbedBuilder errorB = new EmbedBuilder ();
                    errorB.WithColor (Color.Red);
                    errorB.WithTitle ("Error");
                    errorB.WithDescription ("I cannot do that.");
                    await ReplyAsync (string.Empty, false, errorB);
                }
                return;
            }

            EmbedBuilder eb = new EmbedBuilder ();
            eb.WithColor (Color.Green);
            eb.WithTitle ("Success");
            eb.WithDescription ($"{user.Mention}, **{role.Name}** role has been removed from you.");
            await ReplyAsync (string.Empty, false, eb);
        }
    }
}
