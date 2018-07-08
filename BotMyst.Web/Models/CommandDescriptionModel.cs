using System.ComponentModel.DataAnnotations;

namespace BotMyst.Web.Models
{
    public class CommandDescriptionModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Command { get; set; }
        public string Summary { get; set; }
        public string CommandOptionsType { get; set; }

        public int ModuleDescriptionId { get; set; }
        public ModuleDescriptionModel ModuleDescription { get; set; }
    }
}