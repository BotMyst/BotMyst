using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using BotMyst.Web;
using BotMyst.Web.Models;

namespace BotMyst.Web.Controllers
{
    public class DashboardController : Controller
    {
        public async Task<IActionResult> Index ()
        {
            if (User.Identity.IsAuthenticated == false)
                return RedirectToAction ("Login", "Account");

            DiscordAPI api = new DiscordAPI ();
            List<DiscordGuildModel> guilds = (await api.GetUserGuildsAsync (HttpContext)).WherePermissions (8).ToList ();

            foreach (var g in guilds)
            {
                g.HasBotMyst = await api.GetGuildAsync (g.Id) != null;
            }
                
            return View (guilds);
        }
    }
}