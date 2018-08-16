using System.Web;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Discord;
using Discord.Commands;

using BotMyst.Bot.Options.Utility;

namespace BotMyst.Bot.Commands.Utility
{
    public partial class Utility : Module
    {
        [Name ("LMGTFY")]
        [Command ("lmgtfy")]
        [Summary ("Generates a LMGTFY link for people who are too lazy to use the internet")]
        [CommandOptions (typeof (LmgtfyOptions))]
        public async Task Lmgtfy ([Remainder] string search)
        {
            var options = GetOptions<LmgtfyOptions> ();

            string url = $"http://lmgtfy.com/?q={HttpUtility.UrlEncode (search)}";

            EmbedBuilder eb = new EmbedBuilder ();
            eb.Title = "Click this link to get all the knowledge in the world";
            eb.Url = url;

            eb.Build ();

            await SendMessage (options, string.Empty, false, eb);
        }
    }
}