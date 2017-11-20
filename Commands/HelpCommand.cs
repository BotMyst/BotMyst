using Discord;
using Discord.Commands;
using System.Threading.Tasks;

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
                "To view help about a specific command, use \">help <command>\"."
            );

            foreach (ModuleInfo module in this.commands.Modules)
                foreach (CommandInfo command in module.Commands)
                {
                    eb.AddField ($"{command.Name} ({string.Join (", ", command.Aliases)})", command.Summary);
                }

            await ReplyAsync (string.Empty, false, eb);
        }
    }
}
