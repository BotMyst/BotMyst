using Discord.Commands;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BotMyst.Commands
{
    [Summary ("Returns a random YouTube video. Uses an API that's pretty crappy. Will be removed.")]
    public class RandomVideoCommand : ModuleBase
    {
        [Command ("randomvideo")]
        [Summary ("Gets a random YouTube video.")]
        public async Task RandomVideo ()
        {
            string url = $"https://randomyoutube.net/api/getvid?api_token={BotMyst.BotMystConfig.RandomYouTubeToken}";

            string json = string.Empty;
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create (url);
            request.Method = "GET";
            request.Accept = "application/json";

            using (HttpWebResponse response = (HttpWebResponse) (await request.GetResponseAsync ()))
            {
                using (Stream stream = response.GetResponseStream ())
                using (StreamReader reader = new StreamReader (stream))
                    json += await reader.ReadToEndAsync () + "\n";
            }

            RandomVideoObject video = new RandomVideoObject
            {
                // This is the default video if the random API fails.
                // BTW, it's a REALLY good video! Watch it!
                // NOW!
                Vid = "dQw4w9WgXcQ"
            };
            video = JsonConvert.DeserializeObject<RandomVideoObject> (json);

            await ReplyAsync ($"https://www.youtube.com/watch?v={video.Vid}");
        }

        private struct RandomVideoObject
        {
            public string Vid;
        }
    }
}
