using System;
using System.Net;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Discord.Commands;

using Newtonsoft.Json;

using BotMyst.Shared.Models;

namespace BotMyst.Bot
{
    public static class BotMystAPI
    {
        private static readonly string ApiUrl = BotMyst.Configuration ["BotMystApi:Url"];

        public static async Task GenerateModuleDescriptions (CommandService commandService)
        {
            List<ModuleDescription> modules = new List<ModuleDescription> ();

            foreach (ModuleInfo module in commandService.Modules.Where (m => m.IsSubmodule == false))
            {
                ModuleDescription moduleDescription = new ModuleDescription
                {
                    Name = module.Name.Replace ("Module", ""),
                    CommandDescriptions = new List<CommandDescription> ()
                };

                foreach (ModuleInfo commandModule in module.Submodules)
                {
                    CommandInfo command = commandModule.Commands [0];
                
                    moduleDescription.CommandDescriptions.Add (new CommandDescription
                    {
                        Command = command.Name,
                        Summary = command.Summary
                    });
                }

                modules.Add (moduleDescription);
            }

            foreach (ModuleDescription module in modules)
                await PostObject ("moduledescriptions", module);
        }

        public static async Task InitializeCommandOptions (ulong guildId) =>
            await PostObject ($"commandoptions?guildId={guildId}", null);

        private static async Task PostObject (string requestUri, object @object)
        {
            HttpClient httpClient = new HttpClient ();

            httpClient.BaseAddress = new Uri (ApiUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", BotMyst.Configuration ["BotMystApi:AccessToken"]);  

            StringContent stringContent = new StringContent (JsonConvert.SerializeObject (@object), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync (requestUri, stringContent);  
            response.EnsureSuccessStatusCode ();
        }
    }
}