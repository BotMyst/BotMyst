using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using Discord.Commands;

using RestSharp;

using BotMyst.Bot.Models;

namespace BotMyst.Bot
{
    public static class BotMystAPI
    {
        private static readonly string ApiUrl = BotMyst.Configuration ["BotMystApiUrl"];

        public static void GenerateOptions (ulong guildId)
        {
            RestClient client = new RestClient (ApiUrl);

            RestRequest request = new RestRequest ($"generateoptions?guildId={guildId}", Method.POST);
            request.AddHeader ("Authorization", $"Bearer {BotMyst.Configuration ["BotMystApiToken"]}");
            
            IRestResponse response = client.Execute (request);
        }

        public static string GetOptions (Type commandOptionsType, ulong guildId)
        {
            RestClient client = new RestClient(ApiUrl);

            RestRequest request = new RestRequest($"getcommandoptions?commandOptionsType={commandOptionsType.Name}&guildId={guildId}", Method.GET);
            request.AddHeader("Authorization", $"Bearer {BotMyst.Configuration["BotMystApiToken"]}");

            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        public static T GetOptions<T> (ulong guildId) where T : CommandOptions
        {
            string res = GetOptions (typeof (T), guildId);

            T options = JsonConvert.DeserializeObject<T> (res);

            return options;
        }

        public static void SendModuleData (CommandService commands)
        {
            List<ModuleDescriptionModel> modules = new List<ModuleDescriptionModel> ();

            foreach (var m in commands.Modules)
            {
                if (m.Name == "Module")
                    continue;

                ModuleDescriptionModel model = new ModuleDescriptionModel
                {
                    Name = m.Name,
                    CommandDescriptions = new List<CommandDescriptionModel> ()
                };

                for (int i = 0; i < m.Commands.Count; i++)
                {
                    var c = m.Commands [i];

                    CommandDescriptionModel command = new CommandDescriptionModel
                    {
                        Name = c.Name,
                        Command = c.Aliases [0],
                        Summary = c.Summary,
                    };

                    CommandOptionsAttribute at = (CommandOptionsAttribute) c.Attributes.First(a => a.GetType() == typeof(CommandOptionsAttribute));
                    command.CommandOptionsType = at.CommandOptionsType.Name;

                    model.CommandDescriptions.Add (command);
                }

                modules.Add (model);
            }

            RestClient client = new RestClient (ApiUrl);

            RestRequest request = new RestRequest ($"sendmoduledata", Method.POST);
            request.AddHeader ("Authorization", $"Bearer {BotMyst.Configuration ["BotMystApiToken"]}");
            request.AddJsonBody (modules.ToArray ());

            client.Execute (request);
        }
    }
}