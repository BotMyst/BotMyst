using System;
using System.Collections.Generic;

using BotMyst.Web.Models;

namespace BotMyst.Web
{
    public static class DiscordAPIExtensions
    {
        public static IEnumerable<DiscordGuildModel> WherePermissions (this List<DiscordGuildModel> guilds, int permissions)
        {
            foreach (DiscordGuildModel g in guilds)
            {
                if ((g.Permissions & permissions) == permissions)
                    yield return g;
            }
        }
    }
}
