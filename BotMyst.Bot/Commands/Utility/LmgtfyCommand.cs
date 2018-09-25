using System.Web;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace BotMyst.Bot.Commands
{
    public partial class UtilityModule : ModuleBase
    {
        public class LmgtfyCommand : ModuleBase
        {
            [Command ("lmgtfy")]
            [Summary ("Generates a LMGTFY link for people who are too lazy to use the internet.")]
            public async Task Execute ([Remainder] string search)
            {
                string url = $"http://lmgtfy.com/?q={HttpUtility.UrlEncode (search)}";

                EmbedBuilder eb = new EmbedBuilder ();
                eb.Title = "Click this link to get all the knowledge in the world";
                eb.Url = url;

                await ReplyAsync (string.Empty, false, eb.Build ());
            }
        }
    }
}