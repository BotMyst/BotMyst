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
        private ModulesContext _modulesContext;

        public DashboardController (ModulesContext modulesContext)
        {
            _modulesContext = modulesContext;
        }

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

        public async Task<IActionResult> GuildSettings (string id)
        {
            foreach (var module in _modulesContext.Modules)
            {
                System.Console.WriteLine($"{module.Name}:");
            }

            DiscordAPI api = new DiscordAPI ();
            DiscordGuildModel guild = await api.GetGuildAsync (id);

            if (guild == null)
            {
                guild = (await api.GetUserGuildsAsync (HttpContext)).FirstOrDefault (g => g.Id == id);
                guild.HasBotMyst = false;
            }
            else
            {
                guild.HasBotMyst = true;
            }

            return View (guild);
        }
    }
}