using System.Web;
using System.Threading.Tasks;

using Discord.Commands;

namespace BotMyst.Commands
{
    [Summary ("Generates a lmgtfy link for people who are too lazy or stupid to google things.")]
    public class LmgtfyCommand : ModuleBase
    {
        [Command ("lmgtfy"), Summary ("Generates a lmgtfy link for a specified search.")]
        [Alias ("google")]
        public async Task Lmgtfy ([Remainder] string search)
        {
            string url = "http://lmgtfy.com/?q=";
            await ReplyAsync ($"Help is on the way! :rotating_light:\n{url + HttpUtility.UrlEncode (search)}");
        }
    }
}
