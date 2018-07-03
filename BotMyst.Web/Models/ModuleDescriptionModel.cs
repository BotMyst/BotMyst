using System.Collections.Generic;

namespace BotMyst.Web.Models
{
    public class ModuleDescriptionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CommandDescriptionModel> CommandDescriptions { get; set; }
    }
}