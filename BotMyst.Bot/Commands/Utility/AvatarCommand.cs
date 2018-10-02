using System.Web;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

namespace BotMyst.Bot.Commands
{
    public partial class UtilityModule : ModuleBase
    {
        public class AvatarCommand : ModuleBase
        {
            [Command ("avatar")]
            [Summary ("Gets the avatar of a specified user.")]
            public async Task Execute ([Remainder] IGuildUser user)
            {
                string avatarUrl = user.GetAvatarUrl (ImageFormat.Auto, 256);

                string displayName = !string.IsNullOrEmpty (user.Nickname) ? user.Nickname : user.Username;

                if (string.IsNullOrEmpty (user.AvatarId))
                    avatarUrl = "https://discordapp.com/assets/dd4dbc0016779df1378e7812eabaa04d.png";

                Embed e = new EmbedBuilder ()
                    .WithTitle ($"{displayName}'s avatar")
                    .WithImageUrl (avatarUrl)
                    .Build ();

                await ReplyAsync (string.Empty, false, e);
            }
        }
    }
}