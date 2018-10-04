using System;
using System.Threading.Tasks;

using Discord.Commands;

namespace BotMyst.Bot.Commands
{
    public partial class ModerationModule : ModuleBase
    {
        public class ClearCommand : ModuleBase
        {
            [Command ("clear")]
            [Summary ("Clears a specified amount of messages in the current channel.")]
            public async Task Execute (int numberOfMessages)
            {
                throw new NotImplementedException ();
            }
        }
    }
}