using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BotMyst.Web.Models;
using BotMyst.Web.Helpers;
using BotMyst.Web.Discord;
using BotMyst.Shared.Models;
using BotMyst.Web.Models.DatabaseContexts;
using BotMyst.Shared.Models.CommandOptions;
using BotMyst.Shared.Models.CommandOptions.Utility;
using Microsoft.AspNetCore.Authorization;

namespace BotMyst.Web.Controllers
{
    public class CommandOptionsController : Controller
    {
        private ModuleDescriptionsContext moduleDescriptionsContext;
        private CommandOptionsContext commandOptionsContext;

        private IEnumerable<Type> commandOptionTypes;

        public CommandOptionsController (ModuleDescriptionsContext moduleDescriptionsContext, CommandOptionsContext commandOptionsContext)
        {
            this.moduleDescriptionsContext = moduleDescriptionsContext;
            this.commandOptionsContext = commandOptionsContext;
            commandOptionTypes = CommandHelper.GetCommandOptionsTypes ();
        }

        public async Task<IActionResult> Index (ulong guildId, int commandId)
        {
            CommandOptionsModel model = new CommandOptionsModel ();

            model.BotMystGuild = await DiscordAPI.GetBotMystGuildAsync (guildId, HttpContext);

            if (model.BotMystGuild.Guild == null) return NotFound ();

            model.CommandDescription = await moduleDescriptionsContext.CommandDescriptions.SingleOrDefaultAsync (c => c.ID == commandId);

            if (model.CommandDescription == null) return NotFound ();

            model.CommandOptions = (CommandOptions) await commandOptionsContext.FindAsync (commandOptionTypes.First (t => t.Name.ToLower () == model.CommandDescription.Command.ToLower () + "options"), guildId);

            PropertyInfo [] commandOptionsProperties = model.CommandOptions.GetType ().GetProperties ();

            model.Options = new List<OptionDescription> ();

            foreach (PropertyInfo property in commandOptionsProperties)
            {
                if (property.Name == "GuildId" ||
                    property.Name == "Enabled")
                        continue;

                OptionAttribute optionAttribute = (OptionAttribute) property.GetCustomAttribute (typeof (OptionAttribute));

                if (optionAttribute == null)
                    continue;
            
                OptionDescription optionDescription = new OptionDescription
                {
                    // Prettify the option name. Example: "RoleWhitelist" would become "Role Whitelist"
                    Name = Regex.Replace (property.Name, "(\\B[A-Z])", " $1"),
                    Summary = optionAttribute.Summary,
                    OptionType = optionAttribute.OptionType,
                    Value = property.GetValue (model.CommandOptions, null)
                };

                model.Options.Add (optionDescription);
            }

            return View (model);
        }

        public async Task<IActionResult> ToggleCommand (ulong guildId, int commandId)
        {
            if (await UserHelper.CanChangeOptionsAsync (User, HttpContext, guildId) == false) return Unauthorized ();

            CommandDescription description = await moduleDescriptionsContext.CommandDescriptions.SingleOrDefaultAsync (c => c.ID == commandId);
            CommandOptions options = (CommandOptions) await commandOptionsContext.FindAsync (commandOptionTypes.First (t => t.Name.ToLower () == description.Command.ToLower () + "options"), guildId);

            options.Enabled = !options.Enabled;

            await commandOptionsContext.SaveChangesAsync ();

            return Json (options.Enabled);
        }

        public async Task<IActionResult> AddBlobToBlobList (ulong guildId, int commandId, string optionName, string blob)
        {
            if (await UserHelper.CanChangeOptionsAsync (User, HttpContext, guildId) == false) return Unauthorized ();

            CommandDescription description = await moduleDescriptionsContext.CommandDescriptions.SingleOrDefaultAsync (c => c.ID == commandId);
            BaseCommandOptions options = await CommandHelper.GetCommandOptionsAsync<BaseCommandOptions> (guildId, commandOptionsContext, description, commandOptionTypes);

            await CommandHelper.AddItemToOptionStringList<BaseCommandOptions> (options, commandOptionsContext, optionName, blob);

            return Ok ();
        }
    }
}
