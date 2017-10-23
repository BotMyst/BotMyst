using Discord.Commands;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BotMyst.Commands
{
    public class HelpCommand : ModuleBase
    {
        [Command ("help"), Summary ("Lists the commands and their descriptions.")]
        [Alias ("commands")]
        public async Task Help ()
        {
            string finalMessage = "**HELP**\n\n";

            // Get the current executing assembly
            Assembly assembly = Assembly.GetExecutingAssembly ();

            // Get all types that inherit from ModuleBase, which is a neccessary class
            // for all commands
            Type [] moduleTypes = assembly.GetTypes ().Where (t => typeof (ModuleBase).IsAssignableFrom (t)).ToArray ();

            // Get all methods that have the command attribute
            MethodInfo [] commands = moduleTypes.SelectMany (t => t.GetMethods ())
                                                .Where (m => m.GetCustomAttributes (typeof (CommandAttribute), false).Length > 0)
                                                .ToArray ();

            foreach (MethodInfo command in commands)
            {
                string commandName = command.Name.ToLower ();
                string summary = string.Empty;
                string aliases = string.Empty;

                // Get the summary attribute on the command method
                SummaryAttribute summaryAttribute = (SummaryAttribute) command.GetCustomAttribute (typeof (SummaryAttribute), false);
                summary = summaryAttribute?.Text;

                // Get the alias attribute on the command method
                AliasAttribute aliasAttribute = (AliasAttribute) command.GetCustomAttribute (typeof (AliasAttribute), false);

                // If the command doesn't have a summary or aliases then don't
                // include them in the help (obviously). Otherwise this would
                // throw a NullReferenceException.
                if (aliasAttribute != null)
                    aliases = $"({string.Join (", ", aliasAttribute.Aliases)})";

                if (summaryAttribute != null)
                    summary = $"- {summaryAttribute.Text}";

                finalMessage += $"**{commandName ?? ""}** {aliases} {summary}\n";
            }

            await ReplyAsync (finalMessage);
        }
    }
}
