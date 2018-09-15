using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using BotMyst.Bot.Models;
using BotMyst.Bot.Options.Utility;

using Newtonsoft.Json;

using System.Net;
using System.Linq;
using System.Net.Http;
using System.Collections.Generic;

namespace BotMyst.Bot.Commands.Utility
{
    public partial class Utility : Module
    {
        [Name ("Dictionary")]
        [Command ("dict")]
        [Summary ("Defines a given word")]
        [CommandOptions (typeof (DictionaryOptions))]
        public async Task Dictionary ([Remainder] string word)
        {
            var options = GetOptions<DictionaryOptions>();

            string command = "definitions";
            string json = await Json(word, command);

            Definitions defs = JsonConvert.DeserializeObject<Definitions>(json);

            string output = string.Empty;

            if(defs.definitions.Count <= 0)
            {
                    output = $"No {command} found for **{word}**. Please try another!";
            }

            for (int i = 0; i < defs.definitions.Count; i++)
            {
                if (i >= 3)
                    break;

                string define = defs.definitions[i].definition;
                define = define.First().ToString().ToUpper() + define.Substring(1);
                output += $"{i + 1}: {define}. \n";
            }

            var e = new EmbedBuilder();
            e.WithTitle($"{command} for {word}:");
            e.WithDescription(output);
            e.WithColor(new Color(41, 80, 142));

            await SendMessage (options, string.Empty, false, e);
        }

        public async Task<string> Json(string word, string command)
        {
            HttpClient client = new HttpClient();

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get,$"https://wordsapiv1.p.mashape.com/words/{word}/{command}");
            message.Headers.Add("X-Mashape-Key", "wvgpHS0ZdFmshev28LKZkELOH5R9p15nc7RjsnmZxyFlZFazmq");
            message.Headers.Add("X-Mashape-Host", "wordsapiv1.p.mashape.com");  

            HttpResponseMessage responce = await client.SendAsync(message);

            return await responce.Content.ReadAsStringAsync();;                      
        }

        public class Definition
        {
            public string definition { get; set; }
        }

        public class Definitions
        {
            public List<Definition> definitions { get; set; }
        }
    }
}