using Newtonsoft.Json;

namespace BotMyst
{
    public class BotMystConfiguration
    {
        [JsonProperty ("token")]
        public string Token { get; set; }
        [JsonProperty ("dictionaryAppID")]
        public string DictionaryAppID { get; set; }
        [JsonProperty ("dictionaryAppKey")]
        public string DictionaryAppKey { get; set; }
        [JsonProperty ("randomYouTubeToken")]
        public string RandomYouTubeToken { get; set; }
    }
}