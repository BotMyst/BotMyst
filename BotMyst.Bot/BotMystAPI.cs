using System;
using RestSharp;

namespace BotMyst.Bot
{
    public static class BotMystAPI
    {
        private const string ApiUrl = "http://localhost:5000/";

        public static void GenerateOptions (ulong guildId)
        {
            RestClient client = new RestClient (ApiUrl);

            RestRequest request = new RestRequest ($"api/generateoptions?guildId={guildId}", Method.POST);
            request.AddHeader ("Authorization", $"Bearer {BotMyst.Configuration ["BotMystApiToken"]}");
            
            IRestResponse response = client.Execute (request);
        }

        public static void GetOptions (Type commandOptionsType, ulong guildId)
        {
            RestClient client = new RestClient(ApiUrl);

            RestRequest request = new RestRequest($"api/getcommandoptions?commandOptionsType={commandOptionsType.Name}&guildId={guildId}", Method.GET);
            request.AddHeader("Authorization", $"Bearer {BotMyst.Configuration["BotMystApiToken"]}");

            IRestResponse response = client.Execute(request);
        }
    }
}