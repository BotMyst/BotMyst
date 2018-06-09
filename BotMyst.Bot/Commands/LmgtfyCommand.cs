using System.Threading.Tasks;

using Discord.Commands;

namespace BotMyst.Bot.Commands
{
    [Command("LMGTFY", "lmgtfy", "Generates a LMGTFY link for people that are too lazy to use the internet.")]
    public class LmgtfyCommand : Command
    {
        public LmgtfyCommand (CommandContext context) : base (context)
        {
        }

        public override async Task Execute()
        {
            await SendMessage ("LMGTFY");
        }
    }
}