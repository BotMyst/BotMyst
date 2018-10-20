using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BotMyst.Web.Helpers;
using BotMyst.Web.Discord;
using BotMyst.Shared.Models;
using BotMyst.Web.Discord.Models;
using BotMyst.Web.Models.DatabaseContexts;
using BotMyst.Shared.Models.CommandOptions;

namespace BotMyst.Web.Controllers
{
    [Route ("Discord")]
    public class DiscordController : Controller
    {
        private ModuleDescriptionsContext moduleDescriptionsContext;
        private CommandOptionsContext commandOptionsContext;

        private IEnumerable<Type> commandOptionsTypes;

        public DiscordController (ModuleDescriptionsContext moduleDescriptionsContext, CommandOptionsContext commandOptionsContext)
        {
            this.moduleDescriptionsContext = moduleDescriptionsContext;
            this.commandOptionsContext = commandOptionsContext;
            commandOptionsTypes = CommandHelper.GetCommandOptionsTypes ();
        }

        public IActionResult Test ()
        {
            return Json ("hewoo");
        }

        [Route ("AvailableRoles")]
        public async Task<IActionResult> GetAvailableRoles (ulong guildId, int commandId, string optionName)
        {
            if (await UserHelper.CanChangeOptionsAsync (User, HttpContext, guildId) == false) return Unauthorized ();

            CommandDescription commandDescription = await moduleDescriptionsContext.CommandDescriptions.SingleOrDefaultAsync (c => c.ID == commandId);
            if (commandDescription == null) return NotFound ();

            DiscordGuild guild = await DiscordAPI.GetBotGuildAsync (guildId);
            if (guild == null) return NotFound ();

            DiscordRole [] allRoles = guild.Roles;

            BaseCommandOptions commandOptions = await CommandHelper.GetCommandOptionsAsync<BaseCommandOptions> (guildId, commandOptionsContext, commandDescription, commandOptionsTypes);

            PropertyInfo [] properties = commandOptions.GetType ().GetProperties ();
            // Find the property with the specified name (spaces in the option name need to be removed)
            PropertyInfo selectedProperty = properties.FirstOrDefault (p => p.Name.ToLower () == optionName.Replace (" ", "").ToLower ());
            if (selectedProperty == null) return NotFound (selectedProperty);

            string propertyValue = (string) selectedProperty.GetValue (commandOptions);
            if (propertyValue == null) return Json (allRoles);
            string [] excludeRoles = propertyValue.Split (',');

            return Json (allRoles.Where (r => !excludeRoles.Contains (r.Name)));
        }
    }
}