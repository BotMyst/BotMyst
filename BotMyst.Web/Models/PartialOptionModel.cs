namespace BotMyst.Web.Models
{
    public class PartialOptionModel
    {
        public CommandOptionDescriptionModel Option { get; set; }
        public string GuildId { get; set; }
        public int CommandId { get; set; }
    }
}