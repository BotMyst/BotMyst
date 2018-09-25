using System;
using System.IO;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;

namespace BotMyst.Bot
{
    public class BotMyst : IDisposable
    {
        private IConfiguration configuration;

        private DiscordSocketClient client;

        public BotMyst ()
        {
            configuration = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json")
                .Build ();
        }

        public async Task Run ()
        {
            Console.WriteLine ("BotMyst.Bot is starting...");

            client = new DiscordSocketClient ();

            await client.LoginAsync (TokenType.Bot, configuration ["Discord:Token"]);
            await client.SetGameAsync ($"{configuration ["Bot:Prefix"]}help | botmyst.com", null, ActivityType.Watching);
            await client.StartAsync ();

            client.Log += OnLog;

            await Task.Delay (-1);
        }

        /// <summary>
        /// Logs Discord.Net messages.
        /// </summary>
        private Task OnLog (LogMessage arg)
        {
            Console.WriteLine ($"[{DateTime.UtcNow}\t{arg.Source}:{arg.Severity}]\t{arg.Message}");
            return Task.CompletedTask;
        }

        private bool disposedValue = false;

        protected virtual void Dispose (bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client.LogoutAsync ();
                }

                disposedValue = true;
            }
        }

        public void Dispose ()
        {
            Dispose(true);
        }
    }
}