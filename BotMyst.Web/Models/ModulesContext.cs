using System.IO;

using Microsoft.EntityFrameworkCore;

using BotMyst.Bot.Options.Utility;

namespace BotMyst.Web.Models
{
    public class ModulesContext : DbContext
    {
        // Utility Module
        public DbSet<LmgtfyOptions>     LmgtfyOptions { get; set; }
        public DbSet<UserInfoOptions>   UserInfoOptions { get; set; }

        public ModulesContext (DbContextOptions<ModulesContext> options) : base (options) {}

        protected override void OnModelCreating (ModelBuilder builder)
        {
            builder.Entity<LmgtfyOptions> ().ToTable ("LmgtfyOptions");
            builder.Entity<UserInfoOptions> ().ToTable ("UserInfoOptions");
        }
    }
}