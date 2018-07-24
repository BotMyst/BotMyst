using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BotMyst.Web.Models
{
    public class ModuleDescriptionModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<CommandDescriptionModel> CommandDescriptions { get; set; }
    }
}