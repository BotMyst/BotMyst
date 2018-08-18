using System.Linq;

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
    }
}