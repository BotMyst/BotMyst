using System.Linq;
using System.Threading.Tasks;

using BotMyst.Web.Models;

namespace BotMyst.Web
{
    public static class Helper
    {
        public static async Task<DiscordRoleModel []> GetRolesFromString (ulong guildId, string roles)
        {
            DiscordAPI api = new DiscordAPI ();
            DiscordGuildModel guild = await api.GetGuildAsync (guildId.ToString ());
            string [] sep = roles.Split (',');
            DiscordRoleModel [] allRoles = guild.Roles;
            DiscordRoleModel [] res = allRoles.Where (r => sep.Contains (r.Name)).ToArray ();
            return res;
        }
    }
}