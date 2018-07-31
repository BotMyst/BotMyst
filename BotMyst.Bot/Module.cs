using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace BotMyst.Bot
{
    public class Module : ModuleBase
    {
        protected async Task SendMessage<T> (string text, bool isTTS = false, Embed embed = null, RequestOptions options = null) where T : CommandOptions
        {
            T cmdopt = BotMystAPI.GetOptions<T> (Context.Guild.Id);

            if (cmdopt.Dm)
            {
                IDMChannel dm = await Context.User.GetOrCreateDMChannelAsync ();
                await dm.SendMessageAsync (text, isTTS, embed, options);
            }
            else
            {
                await ReplyAsync (text, isTTS, embed, options);
            }
        }
    }
}