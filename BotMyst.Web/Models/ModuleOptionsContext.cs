using Microsoft.EntityFrameworkCore;

using BotMyst.Bot.Options.Utility;

namespace BotMyst.Web.Models
{
    public class ModuleOptionsContext : DbContext
    {
        // Utility Module
        public DbSet<LmgtfyOptions> LmgtfyOptions { get; set; }
        public DbSet<UserInfoOptions> UserInfoOptions { get; set; }
    
        public ModuleOptionsContext (DbContextOptions<ModuleOptionsContext> options) : base(options) { }

        // protected override void OnModelCreating(ModelBuilder builder)
        // {
        //     builder.Entity<LmgtfyOptions>().ToTable("LmgtfyOptions");
        //     builder.Entity<UserInfoOptions>().ToTable("UserInfoOptions");
        // }
    }
}