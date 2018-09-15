using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using BotMyst.Bot.Models;
using BotMyst.Bot.Options.Utility;

namespace BotMyst.Bot.Commands.Utility
{
    public partial class Utility : Module
    {
        [Name ("Avatar")]
        [Command ("avatar")]
        [Summary ("Grabs the avatar of the specified user.")]
        [CommandOptions (typeof (AvatarOptions))]
        public async Task Avatar ([Remainder] IGuildUser user)
        {
            var options = GetOptions<AvatarOptions> ();

            string avatarUrl = user.GetAvatarUrl (ImageFormat.Auto, 512);

            string displayName = !string.IsNullOrEmpty (user.Nickname) ? user.Nickname : user.Username;

            if (string.IsNullOrEmpty (user.AvatarId))
                avatarUrl = "https://discordapp.com/assets/dd4dbc0016779df1378e7812eabaa04d.png";

            Embed e = new EmbedBuilder ()
                .WithTitle ($"{displayName}'s avatar")
                .WithImageUrl (avatarUrl)
                .Build ();

            await SendMessage (options, string.Empty, false, e);
        }
    }
}