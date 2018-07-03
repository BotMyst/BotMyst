namespace BotMyst.Bot
{
    public class CommandDescriptionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Command { get; set; }
        public string Summary { get; set; }
        public string CommandOptionsType { get; set; }
    }
}