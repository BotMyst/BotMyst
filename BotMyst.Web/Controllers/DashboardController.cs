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
        private ModuleOptionsContext _moduleOptionsContext;

        public DashboardController (ModulesContext modulesContext, ModuleOptionsContext moduleOptionsContext)
        {
            _modulesContext = modulesContext;
            _moduleOptionsContext = moduleOptionsContext;
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
            GuildSettingsModel model = new GuildSettingsModel ();

            model.Modules = new List<ModuleDescriptionModel> ();

            foreach (var m in _modulesContext.Modules)
            {
                m.CommandDescriptions = new List<CommandDescriptionModel> (_modulesContext.CommandDescriptions.Where (d => d.ModuleDescriptionId == m.Id));

                foreach (var c in m.CommandDescriptions)
                {
                    c.Enabled = Helper.CheckIfCommandEnabled (_moduleOptionsContext, c.CommandOptionsType, ulong.Parse (id));
                }

                model.Modules.Add (m);
            }

            DiscordAPI api = new DiscordAPI ();
            model.Guild = await api.GetGuildAsync (id);

            if (model.Guild == null)
            {
                model.Guild = (await api.GetUserGuildsAsync (HttpContext)).FirstOrDefault (g => g.Id == id);
                model.Guild.HasBotMyst = false;
            }
            else
            {
                model.Guild.HasBotMyst = true;
            }

            return View (model);
        }
    }
}