using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using BotMyst.Web.Helpers;
using BotMyst.Shared.Models;
using BotMyst.Web.Models.DatabaseContexts;
using BotMyst.Shared.Models.CommandOptions;

namespace BotMyst.Web.Controllers
{
    [Route ("api")]
    public class ApiController : Controller
    {
        private const string AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;

        private ModuleDescriptionsContext moduleDescriptionsContext;
        private CommandOptionsContext commandOptionsContext;

        private IEnumerable<Type> commandOptionsTypes;

        public ApiController (ModuleDescriptionsContext moduleDescriptionsContext, CommandOptionsContext commandOptionsContext)
        {
            this.moduleDescriptionsContext = moduleDescriptionsContext;
            this.commandOptionsContext = commandOptionsContext;

            commandOptionsTypes = CommandHelper.GetCommandOptionsTypes ();
        }

        /// <summary>
        /// You can send a GET request to this endpoint to check if the API works.
        /// </summary>
        [Authorize (AuthenticationSchemes = AuthenticationScheme)]
        [Route ("confirmapi")]
        [HttpGet]
        public IActionResult ConfirmApi ()
        {
            return Ok ("BotMyst API works!");
        }

        [Authorize (AuthenticationSchemes = AuthenticationScheme)]
        [Route ("commandoptions")]
        [HttpPost]
        public async Task<IActionResult> InitializeCommandOptions (ulong guildId)
        {
            foreach (Type commandOptionType in commandOptionsTypes)
            {
                object instance = Activator.CreateInstance (commandOptionType);
                (instance as CommandOptions).GuildId = guildId;
                commandOptionsContext.Add (instance);
            }

            await commandOptionsContext.SaveChangesAsync ();

            return Ok ();
        }

        [Authorize (AuthenticationSchemes = AuthenticationScheme)]
        [Route ("commandoptions")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCommandOption (string commandOptionType, ulong guildId)
        {
            object option = await commandOptionsContext.FindAsync (commandOptionsTypes.First (t => t.Name == commandOptionType), guildId);
            commandOptionsContext.Remove (option);
            await commandOptionsContext.SaveChangesAsync ();

            return Ok ();
        }

        /// <summary>
        /// Creates a new ModuleDescription or updates if an existing one with the same name already exists.
        /// </summary>
        [Authorize (AuthenticationSchemes = AuthenticationScheme)]
        [Route ("moduledescriptions")]
        [HttpPost]
        public async Task<IActionResult> ModuleDescriptions ([FromBody] ModuleDescription moduleDescription)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Try to find a ModuleDescription that already exists with the same name
                    ModuleDescription localModuleDescription = await moduleDescriptionsContext.ModuleDescriptions.SingleOrDefaultAsync (m => m.Name == moduleDescription.Name);

                    // If an existing ModuleDescription isn't found, add it
                    if (localModuleDescription == null)
                    {
                        await moduleDescriptionsContext.AddAsync (moduleDescription);
                    }
                    // Update the existing ModuleDescription
                    else
                    {
                        // Find all CommandDescriptions under the found ModuleDescription
                        List<CommandDescription> localCommandDescriptions = await moduleDescriptionsContext.CommandDescriptions.Where (c => c.ModuleDescriptionID == localModuleDescription.ID).ToListAsync();

                        // If there are any CommandDescriptions that the current database has but aren't in the incoming list, remove them
                        for (int i = 0; i < localCommandDescriptions.Count; i++)
                        {
                            if (!moduleDescription.CommandDescriptions.Any (c => c.Command == localModuleDescription.CommandDescriptions [i].Command))
                                moduleDescriptionsContext.Remove (localCommandDescriptions [i]);
                        }

                        // Add any missing CommandDescriptions
                        for (int i = 0; i < moduleDescription.CommandDescriptions.Count; i++)
                        {
                            if (!localCommandDescriptions.Any (c => c.Command == moduleDescription.CommandDescriptions [i].Command))
                            {
                                localModuleDescription.CommandDescriptions.Add (moduleDescription.CommandDescriptions [i]);
                                break;
                            }
                            // If there already is a CommandDescription and the Summary doesn't match (the only thing that can be different), update it
                            else if (localCommandDescriptions [i].Summary != moduleDescription.CommandDescriptions [i].Summary)
                            {
                                localCommandDescriptions [i].Summary = moduleDescription.CommandDescriptions [i].Summary;
                            }
                        }
                    }

                    await moduleDescriptionsContext.SaveChangesAsync ();

                    return Ok ();
                }
            }
            catch (DbUpdateException ex)
            {
                return BadRequest (ex);
            }

            return BadRequest ();
        }
    }
}