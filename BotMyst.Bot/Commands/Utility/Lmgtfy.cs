using System.Web;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace BotMyst.Bot.Commands.Utility
{
    public partial class Utility : ModuleBase
    {
        [Name ("LMGTFY")]
        [Command ("lmgtfy")]
        [Summary ("Generates a LMGTFY link for people who are too lazy to use the internet")]
        public async Task Lmgtfy ([Remainder] string search)
        {
            string url = $"http://lmgtfy.com/?q={HttpUtility.UrlEncode (search)}";

            EmbedBuilder eb = new EmbedBuilder ();
            eb.Title = "Click this link to get all the knowledge in the world";
            eb.Url = url;

            eb.Build ();

            await ReplyAsync (string.Empty, false, eb);
        }

        [Name("LMDDGTFY")]
        [Command("lmddgtfy")]
        [Summary("Generates a LMGTFY (Duck Duck Go) link for people who are too lazy to use the internet, but still care for their privacy!")]
        public async Task Lmddgtfy ([Remainder] string search)
        {
            string url = $"http://lmgtfy.com/?s=d&q={HttpUtility.UrlEncode(search)}";

            EmbedBuilder eb = new EmbedBuilder();
            eb.Title = "Click this link to get all the knowledge in the world";
            eb.Url = url;

            eb.Build();

            await ReplyAsync(string.Empty, false, eb);
        }
    }
}