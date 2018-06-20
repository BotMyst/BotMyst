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

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite ("Data Source=ModuleOptions.db");
            optionsBuilder.UseSqlite ($"Data Source={Path.Combine (Directory.GetCurrentDirectory (), "BotMyst.Web/ModuleOptions.db")}");
        }
    }
}