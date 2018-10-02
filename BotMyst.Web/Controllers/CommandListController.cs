using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using BotMyst.Web.Models;
using BotMyst.Web.Discord;
using BotMyst.Shared.Models;
using BotMyst.Web.DatabaseContexts;

namespace BotMyst.Web.Controllers
{
    public class CommandListController : Controller
    {
        private ModuleDescriptionsContext moduleDescriptionsContext;

        public CommandListController (ModuleDescriptionsContext moduleDescriptions)
        {
            this.moduleDescriptionsContext = moduleDescriptions;
        }

        public async Task<IActionResult> Index (ulong guildId, string commandSearch)
        {
            ViewData ["CurrentFilter"] = commandSearch;

            CommandList model = new CommandList ();
            model.ModuleDescriptions = new List<ModuleDescription> ();

            BotMystGuild guild = await DiscordAPI.GetBotMystGuildAsync (guildId, HttpContext);
            model.BotMystGuild = guild;

            foreach (ModuleDescription module in moduleDescriptionsContext.ModuleDescriptions)
            {
                ModuleDescription moduleDescriptionCopy = new ModuleDescription
                {
                    Name = module.Name,
                    CommandDescriptions = moduleDescriptionsContext.CommandDescriptions.Where (c => c.ModuleDescriptionID == module.ID).ToList ()
                };

                if (string.IsNullOrEmpty (commandSearch) == false)
                    moduleDescriptionCopy.CommandDescriptions = moduleDescriptionCopy.CommandDescriptions.Where (c => c.Command.ToLower ().Contains (commandSearch.ToLower ())).ToList ();

                // If it doesn't have any command (probably because the search), don't show this module in the page
                if (moduleDescriptionCopy.CommandDescriptions.Count == 0)
                    continue;

                model.ModuleDescriptions.Add (moduleDescriptionCopy);
            }

            return View (model);
        }
    }
}