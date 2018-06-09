using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace BotMyst.Bot
{
    public abstract class Command
    {
        protected CommandContext Context { get; }

        public Command (CommandContext context)
        {
            Context = context;
        }

        public abstract Task Execute ();

        protected async Task SendMessage (string message, bool isTts = false, Embed embed = null, RequestOptions requestOptions = null)
        {
            await Context.Channel.SendMessageAsync (message, isTts, embed, requestOptions);
        }
    }
}