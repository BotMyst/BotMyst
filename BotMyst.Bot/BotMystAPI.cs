using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Discord.Commands;

using RestSharp;

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

            RestClient restClient = new RestClient (ApiUrl);

            RestRequest restRequest = new RestRequest ("moduledescriptions", Method.POST);
            restRequest.AddHeader ("Authorization", $"Bearer {BotMyst.Configuration ["BotMystApi:AccessToken"]}");
            
            foreach (ModuleDescription module in modules)
            {
                restRequest.AddJsonBody (module);

                IRestResponse restResponse = await restClient.ExecuteTaskAsync (restRequest);
                
                if (restResponse.StatusCode != HttpStatusCode.OK)
                    Console.WriteLine ($"[{DateTime.UtcNow}\tBotMystAPI:UnsuccessfulRequest]\t{restResponse.StatusCode.ToString ()}");
            }
        }
    }
}