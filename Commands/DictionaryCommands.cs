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
    [Summary ("Get dictionary entries about a word. You can also get synonyms and antonyms for a word.")]
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
        public async Task Dictionary (string word)
        {
            string url = $"https://od-api.oxforddictionaries.com:443/api/v1/entries/en/{word.ToLower ()}";

            string json = await FetchJson (url);

            if (string.IsNullOrEmpty (json)) return;
            DictionaryData data = DictionaryData.FromJson (json);

            string finalMessage = string.Empty;
            bool hasData = false;
            DictionaryResult result = data.Results [0];

            finalMessage += $"**{result.Word.ToUpper ()}**\n";

            if (result.LexicalEntries [0].Pronunciations != null)
            {
                finalMessage += $"/{result.LexicalEntries [0].Pronunciations [0].PhoneticSpelling}/\n\n";
                hasData = true;
            }

            foreach (DictionaryLexicalEntry lexicalEntry in result.LexicalEntries ?? Enumerable.Empty<DictionaryLexicalEntry> ())
            {
                finalMessage += $"**{lexicalEntry.LexicalCategory.UppercaseFirst ()}**\n";
                foreach (DictionaryEntry entry in lexicalEntry.Entries ?? Enumerable.Empty<DictionaryEntry> ())
                {
                    for (int j = 0; j < entry.Senses.Length; j++)
                    {
                        DictionarySense sense = entry.Senses [j];
                        foreach (string definition in sense.Definitions ?? Enumerable.Empty<string> ())
                        {
                            finalMessage += $"{j + 1}. {definition.UppercaseFirst ()}\n";
                            hasData = true;
                        }
                        foreach (DictionaryExample example in sense.Examples ?? Enumerable.Empty<DictionaryExample> ())
                        {
                            finalMessage += $"    {example.Text.UppercaseFirst ()}\n";
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
                await ReplyAsync ($"No data found for: {word}.");
            }
        }

        [Command ("thesaurus"), Summary ("Gets the synonyms and antonyms for a specified word.")]
        [Alias ("word", "synonym", "antonym")]
        public async Task Thesaurus (string word)
        {
            string url = $"https://od-api.oxforddictionaries.com:443/api/v1/entries/en/{word.ToLower ()}/synonyms;antonyms";

            string json = await FetchJson (url);
            if (string.IsNullOrEmpty (json)) return;

            ThesaurusData data = ThesaurusData.FromJson (json);

            string finalMessage = string.Empty;

            ThesaurusResult result = data.Results [0];
            finalMessage += $"**{result.Word.ToUpper ()}**\n\n";

            foreach (ThesaurusLexicalEntry lexicalEntry in result.LexicalEntries ?? Enumerable.Empty<ThesaurusLexicalEntry> ())
            {
                foreach (ThesaurusEntry entry in lexicalEntry.Entries ?? Enumerable.Empty<ThesaurusEntry> ())
                {
                    int nOfSenses = 1;
                    foreach (ThesaurusSense sense in entry.Senses ?? Enumerable.Empty<ThesaurusSense> ())
                    {
                        if (sense.Examples != null)
                            finalMessage += $"{nOfSenses}. \"{sense.Examples [0].Text.UppercaseFirst ()}\"\n";
                        else
                            finalMessage += $"{nOfSenses}.\n";

                        if (sense.Synonyms != null || sense.Synonyms.Length >= 1)
                        {
                            finalMessage += "**Synonyms**\n";
                        }
                        string synonyms = string.Empty;
                        foreach (DictionaryOnym onym in sense.Synonyms ?? Enumerable.Empty<DictionaryOnym> ())
                        {
                            synonyms += $"{onym.Text}, ";
                        }
                        if (string.IsNullOrEmpty (synonyms) == false)
                        {
                            finalMessage += $"    {synonyms.Remove (synonyms.Length - 2).UppercaseFirst ()}\n";
                        }

                        foreach (ThesaurusSense subsense in sense.Subsenses ?? Enumerable.Empty<ThesaurusSense> ())
                        {
                            finalMessage += "    ";
                            if (subsense.Regions != null)
                            {
                                if (subsense.Regions [0] != null)
                                {
                                    finalMessage += $"___{subsense.Regions [0].UppercaseFirst ()}___ ";
                                }
                            }
                            if (subsense.Registers != null)
                            {
                                if (subsense.Registers [0] != null)
                                {
                                    finalMessage += $"___{subsense.Registers [0].UppercaseFirst ()}___ ";
                                }
                            }
                            string ssynonyms = string.Empty;
                            foreach (DictionaryOnym onym in subsense.Synonyms ?? Enumerable.Empty<DictionaryOnym> ())
                            {
                                ssynonyms += $"{onym.Text}, ";
                            }
                            if (string.IsNullOrEmpty (ssynonyms) == false)
                            {
                                finalMessage += $"{ssynonyms.Remove (ssynonyms.Length - 2).UppercaseFirst ()}\n";
                            }
                        }

                        if (sense.Antonyms != null)
                        {
                            if (sense.Antonyms.Length > 0)
                            {
                                finalMessage += "\n**Antonyms**\n";
                            }
                        }
                        string antonyms = string.Empty;
                        foreach (DictionaryOnym onym in sense.Antonyms ?? Enumerable.Empty<DictionaryOnym> ())
                        {
                            antonyms += $"{onym.Text}, ";
                        }
                        if (string.IsNullOrEmpty (antonyms) == false)
                        {
                            finalMessage += $"    {antonyms.Remove (antonyms.Length - 2).UppercaseFirst ()}\n";
                        }

                        foreach (ThesaurusSense subsense in sense.Subsenses ?? Enumerable.Empty<ThesaurusSense> ())
                        {
                            string santonyms = string.Empty;
                            foreach (DictionaryOnym onym in subsense.Antonyms ?? Enumerable.Empty<DictionaryOnym> ())
                            {
                                santonyms += $"{onym.Text}, ";
                            }

                            if (string.IsNullOrEmpty (santonyms))
                            {
                                continue;
                            }

                            finalMessage += "    ";
                            if (subsense.Regions != null)
                            {
                                if (subsense.Regions [0] != null)
                                {
                                    finalMessage += $"___{subsense.Regions [0].UppercaseFirst ()}___ ";
                                }
                            }
                            if (subsense.Registers != null)
                            {
                                if (subsense.Registers [0] != null)
                                {
                                    finalMessage += $"___{subsense.Registers [0].UppercaseFirst ()}___ ";
                                }
                            }

                            if (string.IsNullOrEmpty (santonyms) == false)
                            {
                                finalMessage += $"{santonyms.Remove (santonyms.Length - 2).UppercaseFirst ()}\n";
                            }
                        }

                        finalMessage += "\n";
                        nOfSenses++;
                    }
                }
            }

            if (finalMessage.Length >= 2000)
            {
                IEnumerable<string> messages = finalMessage.SplitEveryNth (2000);
                foreach (string message in messages)
                {
                    await ReplyAsync (message);
                }
                return;
            }
            await ReplyAsync (finalMessage);
        }

        private async Task<string> FetchJson (string url)
        {
            string json = string.Empty;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create (url);

            request.Headers ["app_id"] = AppID;
            request.Headers ["app_key"] = AppKey;

            request.Method = "GET";
            request.Accept = "application/json";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse) (await request.GetResponseAsync ()))
                {
                    using (Stream stream = response.GetResponseStream ())
                    using (StreamReader reader = new StreamReader (stream))
                        json += await reader.ReadToEndAsync () + "\n";
                }
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpStatusCode code = ((HttpWebResponse) e.Response).StatusCode;
                    if (code == HttpStatusCode.NotFound)
                        await ReplyAsync ("Word not found. Try again using a different word.");
                    if (code == HttpStatusCode.BadRequest)
                        await ReplyAsync ("The word contains unsupported characters. Try again using a different word.");
                }     
            }

            return json;
        }
    }
}