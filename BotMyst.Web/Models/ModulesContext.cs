using System.IO;

using Microsoft.EntityFrameworkCore;

namespace BotMyst.Web.Models
{
    public class ModulesContext : DbContext
    {
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlite ("Data Source=ModuleOptions.db");
            optionsBuilder.UseSqlite ($"Data Source={Path.Combine (Directory.GetCurrentDirectory (), "BotMyst.Web/ModuleOptions.db")}");
        }
    }
}