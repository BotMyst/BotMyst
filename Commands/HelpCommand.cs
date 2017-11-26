using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using System.Collections.Generic;

namespace BotMyst.Commands
{
    public class HelpCommand : ModuleBase
    {
        private readonly CommandService commands;

        public HelpCommand (CommandService commands)
        {
            this.commands = commands;
        }

        [Command ("help"), Summary ("Lists the commands and their descriptions.")]
        [Alias ("commands")]
        public async Task Help ()
        {
            EmbedBuilder eb = new EmbedBuilder ();
            eb.WithColor (Color.Orange);
            eb.WithTitle ("HELP");
            eb.WithDescription
            (
                "To view help about a specific command, use \">command <command>\"."
            );

            foreach (ModuleInfo module in commands.Modules)
                foreach (CommandInfo command in module.Commands)
                {
                    // command.Aliases also returns the name of the command so we will remove that from the list
                    // into another list since command.Aliases is a read only list.
                    List<string> aliasesList = command.Aliases.ToList ();
                    aliasesList.Remove (command.Name);

                    // If there are no aliases don't display the paratheses.
                    string aliases;
                    if (aliasesList.Count == 0)
                        aliases = string.Empty;
                    else
                        aliases = $"({string.Join (", ", aliasesList)})";

                    eb.AddField ($"{command.Name} {aliases}", command.Summary);
                }

            await ReplyAsync (string.Empty, false, eb);
        }

        [Command ("command"), Summary ("Gets help about an individual command.")]
        public async Task Command (string command)
        {
            EmbedBuilder eb = new EmbedBuilder ();
            eb.WithColor (Color.Orange);

            CommandInfo commandInfo = null;

            foreach (ModuleInfo module in commands.Modules)
                foreach (CommandInfo cmnd in module.Commands)
                    if (cmnd.Name == command || cmnd.Aliases.Contains (command))
                        commandInfo = cmnd;

            if (commandInfo == null)
            {
                await ReplyAsync ($"Couldn't find the **{command}** command. Do {BotMyst.CommandPrefix}help for a list of available commands.");
                return;
            }
            else
            {
                eb.WithTitle ($"{commandInfo.Name} ({string.Join (", ", commandInfo.Aliases)})");
                eb.WithDescription ($"Usage for the {commandInfo.Name} command.");

                string @params = string.Empty;

                foreach (ParameterInfo parameter in commandInfo.Parameters)
                    @params += $"<{parameter.Name}> ";

                eb.AddField ($"{BotMyst.CommandPrefix}{commandInfo.Name} {@params}", commandInfo.Summary);
            }

            await ReplyAsync (string.Empty, false, eb);
        }
    }
}
