using Discord.Commands;

using System.Net;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using BotMyst.Data;
using BotMyst.Helpers;

namespace BotMyst.Commands
{
    public class DictionaryCommands : ModuleBase
    {
        private string AppID
        {
            get
            {
                return BotMyst.BotMystConfig.DictionaryAppID;
            }
        }

        private string AppKey
        {
            get
            {
                return BotMyst.BotMystConfig.DictionaryAppKey;
            }
        }

        [Command ("dictionary"), Summary ("Gets the dictionary information about a word.")]
        [Alias ("dict")]
        public async Task Dictionary (string search)
        {
            string url = $"https://od-api.oxforddictionaries.com:443/api/v1/entries/en/{search.ToLower ()}";
        
            string json = await FetchJson (url);
            DictionaryData data = DictionaryData.FromJson (json);

            string finalMessage = string.Empty;
            bool hasData = false;
            DictionaryResult result = data.Results [0];

            finalMessage += $"**{result.Word.ToUpper()}**\n";

            if (result.LexicalEntries [0].Pronunciations != null)
            {
                finalMessage += $"/{result.LexicalEntries [0].Pronunciations [0].PhoneticSpelling}/\n\n";
                hasData = true;
            }

            foreach (DictionaryLexicalEntry lexicalEntry in result.LexicalEntries ?? Enumerable.Empty<DictionaryLexicalEntry>  ())
            {
                finalMessage += $"**{lexicalEntry.LexicalCategory.UppercaseFirst ()}**\n";
                foreach (DictionaryEntry entry in lexicalEntry.Entries ?? Enumerable.Empty<DictionaryEntry> ())
                {
                    for (int j = 0; j < entry.Senses.Length; j++)
                    {
                        DictionarySense sense = entry.Senses [j];
                        foreach (string definition in sense.Definitions ?? Enumerable.Empty<string> ())
                        {
                            finalMessage += $"{j + 1}. {definition.UppercaseFirst ()}.\n";
                            hasData = true;
                        }
                        foreach (DictionaryExample example in sense.Examples ?? Enumerable.Empty<DictionaryExample> ())
                        {
                            finalMessage += $"    {example.Text.UppercaseFirst ()}.\n";
                            hasData = true;
                        }
                        finalMessage += "\n";
                    }
                }
            }

            if (finalMessage.Length >= 2000 && hasData)
            {
                IEnumerable<string> messages = finalMessage.SplitEveryNth (2000);
                foreach (string message in messages)
                {
                    await ReplyAsync (message);
                }
                return;
            }
            else if (hasData)
            {
                await ReplyAsync (finalMessage);
            }
            else
            {
                await ReplyAsync ($"No data found for: {search}.");
            }
        }

        private async Task<string> FetchJson (string url)
        {
            string json = string.Empty;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create (url);

            request.Headers ["app_id"] = AppID;
            request.Headers ["app_key"] = AppKey;

            request.Method = "GET";
            request.Accept = "application/json";

            using (HttpWebResponse response = (HttpWebResponse) (await Task<WebResponse>.Factory.FromAsync (request.BeginGetResponse, request.EndGetResponse, null)))
                using (Stream stream = response.GetResponseStream ())
                    using (StreamReader reader = new StreamReader (stream))
                        json += await reader.ReadToEndAsync () + "\n";

            return json;
        }
    }
}