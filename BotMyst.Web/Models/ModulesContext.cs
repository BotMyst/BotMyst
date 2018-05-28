using System.IO;

using Microsoft.EntityFrameworkCore;

using BotMyst.Bot.Options;

namespace BotMyst.Web.Models
{
    public class ModulesContext : DbContext
    {
        public DbSet<UtilityOptions> UtilityOptions { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite ("Data Source=ModuleOptions.db");
            optionsBuilder.UseSqlite ($"Data Source={Path.Combine (Directory.GetCurrentDirectory (), "BotMyst.Web/ModuleOptions.db")}");
        }
    }
}