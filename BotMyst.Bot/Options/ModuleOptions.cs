using System.ComponentModel.DataAnnotations.Schema;

namespace BotMyst.Bot.Options
{
    public class ModuleOptions
    {
        [DatabaseGenerated (DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }
    }
}