using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;

namespace BotMyst.Bot
{
    public class BotMyst
    {
        public static IConfiguration Configuration { get; set; }

        private DiscordSocketClient client;
        private IServiceProvider services;

        private List<Type> commandTypes = new List<Type> ();

        public BotMyst ()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json");

            Configuration = builder.Build ();
        }

        public async Task Start ()
        {
            client = new DiscordSocketClient ();

            services = new ServiceCollection ()
                    .BuildServiceProvider ();

            await InstallCommands ();

            await client.LoginAsync (TokenType.Bot, Configuration ["discordToken"]);
            await client.SetGameAsync ($"{Configuration ["prefix"]}help botmyst.com");
            await client.StartAsync ();

            client.Log += Log;

            client.Ready += GenerateSettings;

            await Task.Delay (-1);
        }

        private async Task GenerateSettings ()
        {
            // System.Console.WriteLine($"{DateTime.Now.ToString ("HH:mm:ss")} Generating guild settings");

            // foreach (IGuild g in client.Guilds)
            // {
            //     BotMystAPI.GenerateOptions (g.Id);
            // }

            // System.Console.WriteLine($"{DateTime.Now.ToString ("HH:mm:ss")} Successfully generated guild settings for {client.Guilds.Count} guilds.");
        }

        private Task Log (LogMessage arg)
        {
            System.Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task InstallCommands ()
        {
            client.MessageReceived += HandleCommand;

            commandTypes.AddRange (Assembly.GetExecutingAssembly ().GetTypes ().Where (t => t.IsSubclassOf (typeof (Command)) && !t.IsAbstract && t.GetCustomAttributes ().Any (a => a.GetType () == typeof (CommandAttribute))));

            foreach (Type t in commandTypes)
                System.Console.WriteLine(t.Name);
        }

        private async Task HandleCommand (SocketMessage arg)
        {
            SocketUserMessage message = arg as SocketUserMessage;
            string messageString = message.ToString ();

            if (message == null) return;
            if (message.Author.Id == client.CurrentUser.Id) return;

            int argPos = 0;

            if ((message.HasStringPrefix (Configuration ["prefix"], ref argPos) || message.HasMentionPrefix (client.CurrentUser, ref argPos)) == false) return;

            ICommandContext commandContext = new CommandContext (client, message);

            // if the command is just: >test it will return: test
            // if the command is >test arguments it will return: test
            // have to check if it has an empty space
            string commandName = messageString.Substring (Configuration ["prefix"].Length, messageString.Contains (" ") ? messageString.IndexOf (" ") - 1 : messageString.Length - 1);
        }
    }
}