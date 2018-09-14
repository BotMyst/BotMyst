namespace BotMyst.Web.Models
{
    public class BlobPickerModel
    {
        public string GuildId { get; set; }
        public string CommandId { get; set; }
        public string OptionName { get; set; }
        public IDiscordNameable [] Items { get; set; }
    }
}