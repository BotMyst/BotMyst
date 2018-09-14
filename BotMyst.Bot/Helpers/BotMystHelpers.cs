using System;
using System.Linq;
using Discord;
using Discord.WebSocket;

namespace BotMyst.Bot.Helpers
{
    public static class BotMystHelpers
    {
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

        public static bool CheckWhitelistBlacklistChannels (string channelName, string [] whitelistChannels, string [] blacklistChannels)
        {
            bool canRun = false;

            if (whitelistChannels == null ||
                whitelistChannels.Length == 0)
            {
                canRun = true;
            }
            else
            {
                if (whitelistChannels.Contains (channelName))
                    canRun = true;
            }

            if (blacklistChannels == null ||
                blacklistChannels.Length == 0)
            {
                canRun = true;   
            }
            else
            {
                if (blacklistChannels.Contains (channelName))
                    canRun = false;
            }

            return canRun;
        }
    }
}