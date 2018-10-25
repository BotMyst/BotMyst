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

        /// <summary>
        /// Adds an item to a string representing a list in a command option.
        /// Example is RoleWhitelist (Staff,Admin,Bot).
        /// </summary>
        public static async Task AddItemToOptionStringList<T> (T options, CommandOptionsContext commandOptionsContext, string optionName, string value) where T : CommandOptions
        {
            PropertyInfo [] properties = options.GetType ().GetProperties ();
            PropertyInfo propertyToChange = properties.First (p => p.Name == optionName.Replace (" ", ""));
            string currentValue = (string) propertyToChange.GetValue (options);
            string valueToSet;
            if (string.IsNullOrEmpty (currentValue))
                valueToSet = value;
            else
                valueToSet = $"{currentValue},{value}";
            propertyToChange.SetValue (options, valueToSet);
            await commandOptionsContext.SaveChangesAsync ();
        }

        /// <summary>
        /// Removes an item from a string representing a list in a command option.
        /// Example is RoleWhitelist (Staff,Admin,Bot).
        /// If the list doesn't contain said item, no error or exception is thrown.
        /// </summary>
        public static async Task RemoveItemFromOptionStringList<T> (T options, CommandOptionsContext commandOptionsContext, string optionName, string value) where T : CommandOptions
        {
            PropertyInfo [] properties = options.GetType ().GetProperties ();
            PropertyInfo propertyToChange = properties.First (p => p.Name == optionName.Replace (" ", ""));
            string currentValue = (string) propertyToChange.GetValue (options);
            string valueToSet;
            if (string.IsNullOrEmpty (currentValue))
            {
                return;
            }
            else
            {
                List<string> allValues = currentValue.Split (',').ToList ();
                if (allValues.Remove (value) == false) return;
                valueToSet = string.Join (',', allValues);
            }
            propertyToChange.SetValue (options, valueToSet);
            await commandOptionsContext.SaveChangesAsync ();
        }
    }
}