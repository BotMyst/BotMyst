using System.Threading.Tasks;

using Discord.Commands;

namespace BotMyst.Commands
{
    public class LmgtfyCommand : ModuleBase
    {
        [Command ("lmgtfy"), Summary ("Generates a lmgtfy link for a specified search.")]
        [Alias ("help", "google")]
        public async Task Lmgtfy ([Remainder] string search)
        {
            string url = "http://lmgtfy.com/?q=";
            string [] parameters = search.Split (' ');

            foreach (string param in parameters)
                url += $"{param}+";

            await ReplyAsync ($"Help is on the way! :rotating_light:\n{url.Remove (url.Length - 1)}");
        }
    }
}
