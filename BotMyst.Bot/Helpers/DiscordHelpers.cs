using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;

namespace BotMyst.Bot.Helpers
{
    public static class DiscordHelpers
    {
        public static IRole GetUsersHigherstRole (IGuildUser user)
        {
            IReadOnlyCollection<ulong> roleIds = user.RoleIds;

            IRole highestRole = null;
            foreach (ulong id in roleIds)
            {
                IRole role = user.Guild.GetRole(id);
                if (role.Name == "@everyone")
                    continue;

                if (highestRole == null)
                    highestRole = role;
                else if (role.Position > highestRole.Position)
                    highestRole = role;
            }

            return highestRole;
        }

        public static string GetListOfUsersRoles (IGuildUser user)
        {
            string roles = "";
            
            IReadOnlyCollection<ulong> roleIds = user.RoleIds;

            foreach (ulong id in roleIds)
            {
                IRole role = user.Guild.GetRole(id);
                if (role.Name == "@everyone")
                    continue;

                roles += $"{role.Name}, ";
            }

            if (string.IsNullOrEmpty(roles) == false)
                roles = roles.Remove(roles.Length - 2, 2);

            return roles;
        }

        /// <summary>
        /// Checks whether the users passes the whitelist and blacklist role set.
        /// </summary>
        public static bool CheckWhitelistBlacklistRoles (SocketGuildUser user, string [] whitelistRoles, string [] blacklistRoles)
        {
            bool canRun = false;

            if (whitelistRoles == null ||
                whitelistRoles.Length == 0 ||
                whitelistRoles.Contains ("@everyone"))
            {
                canRun = true;
            }
            else
            {
                foreach (var role in user.Roles)
                    if (whitelistRoles.Contains (role.Name))
                        canRun = true;
            }

            if (blacklistRoles == null ||
                blacklistRoles.Length == 0)
            {
                canRun = true;   
            }
            else if (blacklistRoles.Contains ("@everyone"))
            {
                canRun = false;
            }
            else
            {
                foreach (var role in user.Roles)
                    if (blacklistRoles.Contains (role.Name))
                        canRun = false;
            }

            return canRun;
        }
    }
}