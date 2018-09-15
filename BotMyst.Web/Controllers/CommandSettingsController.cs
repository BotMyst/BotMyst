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
using Discord;

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

        public async Task<ActionResult> DisplayBlobPicker (string guildId, string commandId, string optionName, string blobType)
        {
            BlobPickerModel model = new BlobPickerModel
            {
                GuildId = guildId,
                CommandId = commandId,
                OptionName = optionName
            };

            DiscordAPI api = new DiscordAPI ();

            switch (blobType)
            {
                case "RolePicker":
                {
                    model.Items = (await api.GetGuildRolesAsync (guildId)).ToArray ();
                    return PartialView ("RolePicker", model);
                }

                case "ChannelPicker":
                {
                    model.Items = (await api.GetGuildChannelsAsync (guildId)).Where (c => c.Type == (long) ChannelType.Text).ToArray ();
                    return PartialView ("ChannelPicker", model);
                }
            }

            throw new Exception ($"{blobType} is not a valid blob type.");
        }

        public async Task<ActionResult> AddItemToBlobList (string guildId, string commandId, string optionName, string item, string blobType)
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

            string currentItems = ((string) optionProp.GetValue (options, null)).Trim ();
            List<string> allItems = currentItems.Split (',').ToList ();
            allItems.Add (item);
            string newItems = string.Join (',', allItems);
            if (newItems.Length > 0)
            {
                if (newItems [0] == ',')
                    newItems = newItems.Remove (0, 1);
                if (newItems [newItems.Length - 1] == ',')
                    newItems = newItems.Remove (newItems.Length - 2, 1);
            }
            optionProp.SetValue (options, newItems);

            await _moduleOptionsContext.SaveChangesAsync ();

            return RedirectToAction ("Index", new { guildId = guildId, commandId = commandId });
        }

        public async Task<ActionResult> RemoveItemFromBlobList (string guildId, string commandId, string optionName, string item, string blobType)
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

            string currentItems = (string) optionProp.GetValue (options, null);
            List<string> allItems = currentItems.Split (',').ToList ();
            allItems.Remove (item);
            string newItems = string.Join (',', allItems);
            if (newItems.Length > 0)
            {
                if (newItems [0] == ',')
                    newItems = newItems.Remove (0, 1);
                if (newItems [newItems.Length - 1] == ',')
                    newItems = newItems.Remove (newItems.Length - 2, 1);
            }
            optionProp.SetValue (options, newItems);

            await _moduleOptionsContext.SaveChangesAsync ();

            return RedirectToAction ("Index", new { guildId = guildId, commandId = commandId });
        }
    }
}