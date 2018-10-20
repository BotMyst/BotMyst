using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;

using BotMyst.Web.Models;
using BotMyst.Web.Discord;
using BotMyst.Web.Discord.Models;

namespace BotMyst.Web.Helpers
{
    public static class UserHelper
    {
        public static async Task<bool> CanChangeOptionsAsync (ClaimsPrincipal user, HttpContext httpContext, ulong guildId)
        {
            if (user.Identity.IsAuthenticated == false) return false;

            DiscordGuild guild = await DiscordAPI.GetUserGuildAsync (guildId, httpContext);
            if (guild == null) return false;
            if (guild.HasAdministratorPermission () == false) return false;

            return true;
        }
    }
}