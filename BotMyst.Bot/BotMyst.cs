using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BotMyst.Bot
{
    public class BotMyst : IDisposable
    {
        public static IConfiguration Configuration { get; private set; }

        private DiscordSocketClient client;
        private IServiceProvider serviceProvider;
        private CommandService commandService;

        public BotMyst ()
        {
            Configuration = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json")
                .Build ();
        }

        public async Task Run ()
        {
            Console.WriteLine ("BotMyst.Bot is starting...");

            client = new DiscordSocketClient ();
            serviceProvider = new ServiceCollection ().BuildServiceProvider ();
            commandService = new CommandService (new CommandServiceConfig { LogLevel = LogSeverity.Info, ThrowOnError = true });

            await client.LoginAsync (TokenType.Bot, Configuration ["Discord:Token"]);
            await client.SetGameAsync ($"{Configuration ["Bot:Prefix"]}help | botmyst.com", null, ActivityType.Watching);
            await client.StartAsync ();

            client.Log += OnLog;

            client.MessageReceived += HandleMessage;

            client.JoinedGuild += OnJoinedGuild;

            await commandService.AddModulesAsync (Assembly.GetEntryAssembly ());

            await BotMystAPI.GenerateModuleDescriptions (commandService);

            await Task.Delay (-1);
        }

        private async Task HandleMessage (SocketMessage arg)
        {
            SocketUserMessage message = arg as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;

            if ((message.HasStringPrefix (Configuration ["Bot:Prefix"], ref argPos) || message.HasMentionPrefix (client.CurrentUser, ref argPos)) == false) return;

            CommandContext context = new CommandContext (client, message);
            CommandInfo executedCommand = commandService.Search (context, argPos).Commands [0].Command;

            IResult result = await commandService.ExecuteAsync(context, argPos, serviceProvider);
            if (result.IsSuccess == false)
            {
                if (result.Error == CommandError.UnknownCommand) return;
                if (result.Error == CommandError.BadArgCount)
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    eb.WithTitle("ERROR: Bad argument count");
                    eb.WithColor(Color.Red);
                    await context.Channel.SendMessageAsync(string.Empty, false, eb.Build ());
                }
                else
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    eb.WithTitle("Error");
                    eb.WithDescription(result.ErrorReason);
                    eb.WithColor(Color.Red);
                    await context.Channel.SendMessageAsync(string.Empty, false, eb.Build ());
                }
            }
        }

        private Task OnJoinedGuild (SocketGuild arg)
        {
            BotMystAPI.InitializeCommandOptions (arg.Id).RunSynchronously ();
            return Task.CompletedTask;
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