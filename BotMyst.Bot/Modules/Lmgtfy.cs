using System.Web;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;

namespace BotMyst.Bot.Modules
{
    [Summary ("Generates a lmgtfy link for people too lazy / stupid to google on their own")]
    public class Lmgtfy : ModuleBase
    {
        [Command ("lmgtfy")]
        [Alias ("google")]
        [Summary ("Generates a lmgtfy link for people too lazy / stupid to google on their own")]
        public async Task Search ([Remainder] string search)
        {
            string url = $"https://lmgtfy.com/?q={HttpUtility.UrlEncode (search)}";

            EmbedBuilder embed = new EmbedBuilder ();
            embed.Title = "LMGTFY (Google)";
            embed.Description = url;

            await ReplyAsync ("", false, embed.Build ());
        }
    }
}