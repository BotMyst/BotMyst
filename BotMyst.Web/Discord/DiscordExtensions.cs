using System.Linq;
using System.Collections.Generic;

using BotMyst.Web.Discord.Models;

namespace BotMyst.Web.Discord
{
    public static class DiscordExtensions
    {
        public static IEnumerable<DiscordGuild> WherePermissions (this List<DiscordGuild> guilds, int permissions) =>
            guilds.Where (g => (g.Permissions & permissions) == permissions);

        public static bool HasAdministratorPermission (this DiscordGuild guild) =>
            (guild.Permissions & 8) == 8;
    }
}