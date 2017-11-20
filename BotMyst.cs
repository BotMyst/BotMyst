using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Newtonsoft.Json;

using Microsoft.Extensions.DependencyInjection;

namespace BotMyst
{
    public class BotMyst
    {
        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;

        private string commandPrefix = ">";

        public static BotMystConfiguration BotMystConfig;
        private string token;
        
        public async Task Start ()
        {
            client = new DiscordSocketClient ();
            commands = new CommandService ();

            services = new ServiceCollection ()
                .AddSingleton (commands)
                .BuildServiceProvider ();

            // Deserialize the config file and get the token (don't wanna hard code it for obvious reasons)
            byte [] configTxt = File.ReadAllBytes ("BotMystConfig.json");
            using (MemoryStream stream = new MemoryStream (configTxt))
            using (StreamReader reader = new StreamReader (stream))
            {
                string json = await reader.ReadToEndAsync ();
                BotMystConfig = JsonConvert.DeserializeObject<BotMystConfiguration> (json);
                token = BotMystConfig.Token;
            }

            services = new ServiceCollection ().BuildServiceProvider ();

            await InstallCommands ();

            await client.LoginAsync (TokenType.Bot, BotMystConfig.Token);
            await client.SetGameAsync (">help");
            await client.StartAsync ();

            Console.WriteLine ("BotMyst is ready.");

            await Task.Delay (-1);
        }

        private async Task InstallCommands ()
        {
            client.MessageReceived += HandleCommand;
            await commands.AddModulesAsync (Assembly.GetEntryAssembly ());
        }

        private async Task HandleCommand (SocketMessage msg)
        {
            SocketUserMessage message = msg as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;

            if ((message.HasStringPrefix (commandPrefix, ref argPos) || message.HasMentionPrefix (client.CurrentUser, ref argPos)) == false) return;

            CommandContext context = new CommandContext (client, message);
            CommandInfo executedCommand = commands.Search (context, argPos).Commands [0].Command;

            if (BotMystConfig.DisabledCommands != null)
            {
                // Convert all command names to lower just to be sure.
                string [] disabledCommands = BotMystConfig.DisabledCommands.Select (i => i.ToLower ()).ToArray ();
                // Check if the current executing command is disabled.
                if (disabledCommands.Contains (executedCommand.Name.ToLower ()))
                    return;
            }

            IResult result = await commands.ExecuteAsync (context, argPos, services);
            if (result.IsSuccess == false)
            {
                if (result.Error == CommandError.UnknownCommand) return;
                await context.Channel.SendMessageAsync (result.ErrorReason);
            }
        }
    }
}