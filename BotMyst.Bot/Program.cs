using System.Threading.Tasks;

namespace BotMyst.Bot
{
    public class Program
    {
        public static async Task Main ()
        {
            using (BotMyst bot = new BotMyst ())
                await bot.Run ();
        }
    }
}
