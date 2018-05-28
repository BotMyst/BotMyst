using System.ComponentModel.DataAnnotations.Schema;

namespace BotMyst.Bot.Options
{
    public class LmgtfyOptions : CommandOptions
    {
        [DatabaseGenerated (DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
        public string SearchEngine { get; set; } = "";
        public bool InternetExplainer { get; set; } = false;
    }
}