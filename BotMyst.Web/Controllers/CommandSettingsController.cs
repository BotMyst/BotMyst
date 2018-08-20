using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using BotMyst.Bot;
using BotMyst.Web.Models;
using BotMyst.Bot.Models;
using BotMyst.Web.Helpers;
using BotMyst.Bot.Models.DisplayAttributes;

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
                DisplayAttribute displayAttribute = (DisplayAttribute) attributes.First (a => a.GetType ().IsSubclassOf (typeof (DisplayAttribute)));

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

        public async Task<IActionResult> ToggleValue (string guildId,
                                                     int commandId,
                                                     string optionName,
                                                     string optionSummary,
                                                     string optionType,
                                                     bool optionValue)
        {
            var description = _modulesContext.CommandDescriptions.First (c => c.Id == commandId);
            var options = ApiHelpers.GetCommandOptions (_moduleOptionsContext, description.CommandOptionsType, ulong.Parse (guildId));

            var properties = options.GetType ().GetProperties ();

            PropertyInfo optionProp = null;

            foreach (var prop in properties)
            {
                var attr = (CommandOptionNameAttribute) prop.GetCustomAttribute (typeof (CommandOptionNameAttribute));
                if (attr != null && attr.Name == optionName)
                    optionProp = prop;
            }

            optionProp.SetValue (options, !optionValue);

            await _moduleOptionsContext.SaveChangesAsync ();

            return RedirectToAction ("Index", new { guildId = guildId, commandId = commandId });
        }

        public async Task<ActionResult> DisplayRolePicker (string guildId, string commandId, string optionName)
        {
            RolePickerModel model = new RolePickerModel
            {
                GuildId = guildId,
                CommandId = commandId,
                OptionName = optionName
            };

            DiscordAPI api = new DiscordAPI ();
            model.Roles = (await api.GetGuildRolesAsync (guildId)).ToArray ();

            return PartialView ("RolePicker", model);
        }

        public async Task<ActionResult> AddRoleToRoleList (string guildId, string commandId, string optionName, string roleName)
        {
            var description = _modulesContext.CommandDescriptions.First (c => c.Id == int.Parse (commandId));
            var options = ApiHelpers.GetCommandOptions (_moduleOptionsContext, description.CommandOptionsType, ulong.Parse (guildId));

            var properties = options.GetType ().GetProperties ();

            PropertyInfo optionProp = null;

            foreach (var prop in properties)
            {
                var attr = (CommandOptionNameAttribute) prop.GetCustomAttribute (typeof (CommandOptionNameAttribute));
                if (attr != null && attr.Name == optionName)
                    optionProp = prop;
            }

            string currentRoles = ((string) optionProp.GetValue (options, null)).Trim ();
            List<string> allRoles = currentRoles.Split (',').ToList ();
            allRoles.Add (roleName);
            string newRoles = string.Join (',', allRoles);
            optionProp.SetValue (options, newRoles);

            await _moduleOptionsContext.SaveChangesAsync ();

            return RedirectToAction ("Index", new { guildId = guildId, commandId = commandId });
        }

        public async Task<ActionResult> RemoveRoleFromRoleList (string guildId, string commandId, string optionName, string roleName)
        {
            var description = _modulesContext.CommandDescriptions.First (c => c.Id == int.Parse (commandId));
            var options = ApiHelpers.GetCommandOptions (_moduleOptionsContext, description.CommandOptionsType, ulong.Parse (guildId));

            var properties = options.GetType ().GetProperties ();

            PropertyInfo optionProp = null;

            foreach (var prop in properties)
            {
                var attr = (CommandOptionNameAttribute) prop.GetCustomAttribute (typeof (CommandOptionNameAttribute));
                if (attr != null && attr.Name == optionName)
                    optionProp = prop;
            }

            string currentRoles = (string) optionProp.GetValue (options, null);
            List<string> allRoles = currentRoles.Split (',').ToList ();
            allRoles.Remove (roleName);
            string newRoles = string.Join (',', allRoles);
            optionProp.SetValue (options, newRoles);

            await _moduleOptionsContext.SaveChangesAsync ();

            return RedirectToAction ("Index", new { guildId = guildId, commandId = commandId });
        }
    }
}