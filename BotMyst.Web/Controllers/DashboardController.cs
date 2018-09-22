using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using BotMyst.Web.Discord;

namespace BotMyst.Web.Controllers
{
    public class DashboardController : Controller
    {
        public async Task<IActionResult> Index ()
        {
            if (User.Identity.IsAuthenticated == false)
                return RedirectToAction ("Login", "Account");

            return View (await DiscordAPI.GetBotMystGuildsAsync (HttpContext));
        }
    }
}