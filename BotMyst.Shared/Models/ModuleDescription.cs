using System.Collections.Generic;

namespace BotMyst.Shared.Models
{
    public class ModuleDescription
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<CommandDescription> CommandDescriptions { get; set; }
    }
}