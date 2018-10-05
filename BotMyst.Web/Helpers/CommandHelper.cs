using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using BotMyst.Shared.Models.CommandOptions;

namespace BotMyst.Web.Helpers
{
    public static class CommandHelper
    {
        public static IEnumerable<Type> GetCommandOptionsTypes ()
        {
            Assembly assembly = typeof (CommandOptions).Assembly;
            return assembly.GetTypes ().Where (t => t.IsSubclassOf (typeof (CommandOptions)) && t.Name != nameof (BaseCommandOptions));
        }
    }
}