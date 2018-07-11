using System.Reflection;
using System.Collections.Generic;

using BotMyst.Bot;
using BotMyst.Web.Models;

namespace BotMyst.Web.Helpers
{
    public static class ApiHelpers
    {
        public static CommandOptions GetCommandOptions (ModuleOptionsContext moduleOptionsContext, string commandOptionsType, ulong guildId)
        {
            CommandOptions result = null;

            foreach (PropertyInfo p in typeof (ModuleOptionsContext).GetProperties ())
            {
                if (p.PropertyType.GenericTypeArguments.Length == 1 && p.PropertyType.GenericTypeArguments [0].Name == commandOptionsType)
                {
                    dynamic dbSet = p.GetValue (moduleOptionsContext);

                    List<CommandOptions> commandOptions = new List<CommandOptions> ();
                    foreach (var s in dbSet)
                    {
                        commandOptions.Add (s);
                    }

                    return commandOptions.Find (c => c.GuildId == guildId);
                }
            }

            return result;
        }
    }
}