using System;
using System.IO;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Newtonsoft.Json;

namespace BotMyst
{
    public class BotMyst
    {
        private DiscordSocketClient client;

        private BotMystConfiguration config;
        private string token;
        
        public async Task Start ()
        {
            client = new DiscordSocketClient ();

            // Deserialize the config file and get the token (don't wanna hard code it for obvious reasons)
            using (StreamReader reader = new StreamReader ("BotMystConfig.json"))
            {
                string json = await reader.ReadToEndAsync ();
                config = JsonConvert.DeserializeObject<BotMystConfiguration> (json);
                token = config.Token;
            }

            await client.LoginAsync (TokenType.Bot, config.Token);
            await client.SetGameAsync ("");
            await client.StartAsync ();

            Console.WriteLine ("BotMyst is ready.");

            await Task.Delay (-1);
        }
    }
}