using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using BotMyst.Shared.Models;
using BotMyst.Shared.Models.CommandOptions;
using BotMyst.Web.Models.DatabaseContexts;

namespace BotMyst.Web.Helpers
{
    public static class CommandHelper
    {
        public static IEnumerable<Type> GetCommandOptionsTypes ()
        {
            Assembly assembly = typeof (CommandOptions).Assembly;
            return assembly.GetTypes ().Where (t => t.IsSubclassOf (typeof (CommandOptions)) && t.Name != nameof (BaseCommandOptions));
        }

        public static async Task<T> GetCommandOptionsAsync<T> (ulong guildId,
                                                                         CommandOptionsContext commandOptionsContext,
                                                                         CommandDescription commandDescription,
                                                                         IEnumerable<Type> commandOptionsTypes) where T : CommandOptions
        {
            return (T) await commandOptionsContext.FindAsync (commandOptionsTypes.First (t => t.Name.ToLower () == commandDescription.Command.ToLower () + "options"), guildId);
        }
    }
}