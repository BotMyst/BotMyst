using System.Threading.Tasks;
using Discord.Commands;

namespace BotMyst.Bot.Commands
{
    [Command("User Info", "userinfo", "Returns information about a user.")]
    public class UserInfoCommand : Command
    {
        public UserInfoCommand (CommandContext context) : base (context)
        {
        }

        public override async Task Execute()
        {
            await SendMessage ("User Info");
        }
    }
}