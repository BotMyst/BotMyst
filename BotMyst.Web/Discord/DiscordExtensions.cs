using System.Linq;
using System.Threading.Tasks;
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

        public static async Task<IEnumerable<DiscordRole>> ToDiscordRolesAsync (this string input, ulong guildId)
        {
            if (string.IsNullOrEmpty (input)) return null;

            string [] roles = input.Split (',');

            DiscordGuild guild = await DiscordAPI.GetBotGuildAsync (guildId);

            return guild.Roles.Where (r => roles.Contains (r.Name));
        }
    }
}