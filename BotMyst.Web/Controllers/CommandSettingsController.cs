using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using BotMyst.Bot;
using BotMyst.Web.Models;
using BotMyst.Web.Helpers;
using System.Reflection;
using System.Collections.Generic;

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

            model.CommandOptionDescriptions = new List<CommandOptionDescriptionModel> ();

            PropertyInfo [] commandProperties = model.CommandOptions.GetType ().GetProperties ();

            foreach (var prop in commandProperties)
            {
                if (prop.Name == "GuildId" || prop.Name == "Enabled")
                    continue;

                var attributes = prop.GetCustomAttributes ();

                CommandOptionNameAttribute nameAttribute = (CommandOptionNameAttribute) attributes.First (a => a.GetType () == typeof (CommandOptionNameAttribute));
                CommandOptionSummaryAttribute summaryAttribute = (CommandOptionSummaryAttribute) attributes.First (a => a.GetType () == typeof (CommandOptionSummaryAttribute));
                DisaplayAttribute displayAttribute = (DisaplayAttribute) attributes.First (a => a.GetType ().IsSubclassOf (typeof (DisaplayAttribute)));

                CommandOptionDescriptionModel commandDesc = new CommandOptionDescriptionModel
                {
                    Name = nameAttribute.Name,
                    Summary = summaryAttribute.Summary,
                    OptionType = displayAttribute.GetType ().Name,
                    Value = prop.GetValue (model.CommandOptions, null)
                };

                model.CommandOptionDescriptions.Add (commandDesc);
            }

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

        public async Task<ActionResult> ChangeValue (string guildId, string commandId, string optionName)
        {
            

            return RedirectToAction ("Index", new { guildId = guildId, commandId = commandId });
        }
    }
}