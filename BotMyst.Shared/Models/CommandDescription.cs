namespace BotMyst.Shared.Models
{
    public class CommandDescription
    {
        public int ID { get; set; }
        public string Command { get; set; }
        public string Summary { get; set; }
        public int ModuleDescriptionID { get; set; }

        public ModuleDescription ModuleDescription { get; set; }
    }
}