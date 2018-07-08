using System.IO;

using Microsoft.EntityFrameworkCore;

using BotMyst.Bot.Options.Utility;

namespace BotMyst.Web.Models
{
    public class ModulesContext : DbContext
    {
        public DbSet<ModuleDescriptionModel> Modules { get; set; }
        public DbSet<CommandDescriptionModel> CommandDescriptions { get; set; }

        public ModulesContext (DbContextOptions<ModulesContext> options) : base (options) { }
    }
}