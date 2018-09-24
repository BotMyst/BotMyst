using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using BotMyst.Web.Models;
using BotMyst.Web.Discord;

namespace BotMyst.Web.Controllers
{
    public class CommandListController : Controller
    {
        public async Task<IActionResult> Index (ulong guildId)
        {
            BotMystGuild guild = await DiscordAPI.GetBotMystGuildAsync (guildId, HttpContext);

            return View (guild);
        }
    }
}