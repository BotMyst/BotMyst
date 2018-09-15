using System;
using System.Linq;
using System.Threading.Tasks;
using BotMyst.Bot.Models;
using BotMyst.Web.Helpers;
using BotMyst.Web.Models;

namespace BotMyst.Web
{
    public static class Helper
    {
        public static async Task<T []> GetDiscordObjectsFromString<T> (ulong guildId, string value) where T : IDiscordNameable
        {
            DiscordAPI api = new DiscordAPI ();
            DiscordGuildModel guild = await api.GetGuildAsync (guildId.ToString ());

            string [] sep = value.Split (',');
            
            IDiscordNameable [] allObjects = null;

            if (typeof (T) == typeof (DiscordChannelModel))
            {
                allObjects = (await api.GetGuildChannelsAsync (guildId.ToString ())).Cast<IDiscordNameable> ().ToArray ();
            }
            else if (typeof (T) == typeof (DiscordRoleModel))
            {
                allObjects = guild.Roles.Cast<IDiscordNameable> ().ToArray ();
            }

            IDiscordNameable [] res = allObjects.Where (c => sep.Contains (c.Name)).ToArray ();

            return res.Cast<T> ().ToArray ();
        }

        public static bool CheckIfCommandEnabled (ModuleOptionsContext moduleOptionsContext, string commandOptionsType, ulong guildId)
        {
            CommandOptions options = ApiHelpers.GetCommandOptions (moduleOptionsContext, commandOptionsType, guildId);
            return options.Enabled;
        }
    }
}