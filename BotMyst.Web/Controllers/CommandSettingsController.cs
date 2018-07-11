using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using BotMyst.Bot;
using BotMyst.Web.Models;
using BotMyst.Web.Helpers;

namespace BotMyst.Web.Controllers
{
    public class CommandSettingsController : Controller
    {
        private ModulesContext _modulesContext;
        private ModuleOptionsContext _moduleOptionsContext;

        public CommandSettingsController (ModulesContext modulesContext, ModuleOptionsContext moduleOptionsContext)
        {
            _modulesContext = modulesContext;
            _moduleOptionsContext = moduleOptionsContext;
        }

        public async Task<IActionResult> Index (string guildId, string commandId)
        {
            CommandSettingsModel model = new CommandSettingsModel ();

            model.Guild = await new DiscordAPI ().GetGuildAsync (guildId);

            model.CommandDescription = _modulesContext.CommandDescriptions.First (c => c.Id == int.Parse (commandId));
            model.CommandOptions = ApiHelpers.GetCommandOptions (_moduleOptionsContext, model.CommandDescription.CommandOptionsType, ulong.Parse (guildId));

            return View (model);
        }

        public async Task<ActionResult> ToggleCommand (string guildId, string commandId)
        {
            var description = _modulesContext.CommandDescriptions.First (c => c.Id == int.Parse (commandId));
            var options = ApiHelpers.GetCommandOptions (_moduleOptionsContext, description.CommandOptionsType, ulong.Parse (guildId));

            options.Enabled = !options.Enabled;

            await _moduleOptionsContext.SaveChangesAsync ();

            return RedirectToAction ("Index", new { guildId = guildId, commandId = commandId });
        }
    }
}