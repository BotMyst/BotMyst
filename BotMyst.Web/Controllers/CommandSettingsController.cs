using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using BotMyst.Web.Models;

namespace BotMyst.Web.Controllers
{
    public class CommandSettingsController : Controller
    {
        private ModulesContext _modulesContext;

        public CommandSettingsController (ModulesContext modulesContext)
        {
            _modulesContext = modulesContext;
        }

        public async Task<IActionResult> Index (string guildId, string commandId)
        {
            CommandSettingsModel model = new CommandSettingsModel ();

            model.Guild = await new DiscordAPI ().GetGuildAsync (guildId);

            System.Console.WriteLine(commandId);

            model.CommandDescription = _modulesContext.CommandDescriptions.First (c => c.Id == int.Parse (commandId));

            return View (model);
        }
    }
}