using Microsoft.EntityFrameworkCore;

using BotMyst.Web.Models;
using BotMyst.Shared.Models;

namespace BotMyst.Web.DatabaseContexts
{
    public class ModuleDescriptionsContext : DbContext
    {
        public ModuleDescriptionsContext (DbContextOptions<ModuleDescriptionsContext> options) : base (options)
        {
        }

        public DbSet<ModuleDescription> ModuleDescriptions { get; set; }
    }
}