using System.ComponentModel.DataAnnotations;

namespace BotMyst.Shared.Models.CommandOptions
{
    public class CommandOptions
    {
        [Key]
        public ulong GuildId { get; set; }
        public bool Enabled { get; set; }
    }
}
