using System;
using System.IO;
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

        private BotMystConfiguration config;
        private string token;
        
        public async Task Start ()
        {
            client = new DiscordSocketClient ();
            commands = new CommandService ();

            // Deserialize the config file and get the token (don't wanna hard code it for obvious reasons)
            using (StreamReader reader = new StreamReader ("BotMystConfig.json"))
            {
                string json = await reader.ReadToEndAsync ();
                config = JsonConvert.DeserializeObject<BotMystConfiguration> (json);
                token = config.Token;
            }

            services = new ServiceCollection ().BuildServiceProvider ();

            await InstallCommands ();

            await client.LoginAsync (TokenType.Bot, config.Token);
            await client.SetGameAsync ("");
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
            IResult result = await commands.ExecuteAsync (context, argPos, services);
            if (result.IsSuccess == false)
                await context.Channel.SendMessageAsync (result.ErrorReason);
        }
    }
}