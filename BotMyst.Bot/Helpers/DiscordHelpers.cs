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
    }
}