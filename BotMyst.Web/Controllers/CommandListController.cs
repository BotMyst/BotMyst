using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using BotMyst.Web.Models;
using BotMyst.Web.Discord;
using BotMyst.Web.DatabaseContexts;
using BotMyst.Shared.Models;
using System.Collections.Generic;
using System.Linq;

namespace BotMyst.Web.Controllers
{
    public class CommandListController : Controller
    {
        private ModuleDescriptionsContext moduleDescriptionsContext;

        public CommandListController (ModuleDescriptionsContext moduleDescriptions)
        {
            this.moduleDescriptionsContext = moduleDescriptions;
        }

        public async Task<IActionResult> Index (ulong guildId)
        {
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
                model.ModuleDescriptions.Add (moduleDescriptionCopy);
            }

            return View (model);
        }
    }
}