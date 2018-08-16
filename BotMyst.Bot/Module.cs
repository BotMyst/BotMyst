using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace BotMyst.Bot
{
    public class Module : ModuleBase
    {
        protected async Task SendMessage (CommandOptions options, string text, bool isTTS = false, Embed embed = null, RequestOptions requestOptions = null)
        {
            if (options.Dm)
            {
                IDMChannel dm = await Context.User.GetOrCreateDMChannelAsync ();
                await dm.SendMessageAsync (text, isTTS, embed, requestOptions);
            }
            else
            {
                await ReplyAsync (text, isTTS, embed, requestOptions);
            }
        }

        protected T GetOptions<T> () where T : CommandOptions
            => BotMystAPI.GetOptions<T> (Context.Guild.Id);
    }
}