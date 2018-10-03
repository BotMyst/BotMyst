using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using BotMyst.Shared.Models.CommandOptions;

namespace BotMyst.Web.Models.DatabaseContexts
{
    public class CommandOptionsContext : DbContext
    {
        public CommandOptionsContext (DbContextOptions<CommandOptionsContext> options) : base (options)
        {
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            Assembly assembly = typeof (CommandOptions).Assembly;
            List<Type> commandOptionTypes = assembly.GetTypes ().Where (t => t.IsSubclassOf (typeof (CommandOptions))).ToList ();

            foreach (Type commandOptionType in commandOptionTypes)
            {
                modelBuilder.Entity (commandOptionType).ToTable (commandOptionType.Name);
            }
        }
    }
}