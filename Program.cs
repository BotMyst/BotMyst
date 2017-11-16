using System.Threading.Tasks;

namespace BotMyst
{
    public class Program
    {
        public static async Task Main ()
        {
            BotMyst bot = new BotMyst ();
            await bot.Start ();

            await Task.Delay (-1);
        }
    }
}
