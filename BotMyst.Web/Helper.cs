using System.Linq;
using BotMyst.Web.Models;

namespace BotMyst.Web
{
    public static class Helper
    {
        public static DiscordRoleModel [] GetRolesFromString (DiscordGuildModel guild, string roles)
        {
            string [] sep = roles.Split (',');
            DiscordRoleModel [] allRoles = guild.Roles;
            DiscordRoleModel [] res = allRoles.Where (r => sep.Contains (r.Name)).ToArray ();
            return res;
        }
    }
}