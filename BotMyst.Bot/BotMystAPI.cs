using RestSharp;

namespace BotMyst.Bot
{
    public static class BotMystAPI
    {
        private const string ApiUrl = "http://localhost:5000/";

        public static void GenerateOptions (ulong guildId)
        {
            RestClient client = new RestClient (ApiUrl);

            RestRequest request = new RestRequest ($"api/modules/generateoptions?guildId={guildId}", Method.POST);
            request.AddHeader ("Authorization", $"Bearer {BotMyst.Configuration ["BotMystApiToken"]}");
            
            IRestResponse response = client.Execute (request);
        }
    }
}