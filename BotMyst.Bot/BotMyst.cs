using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using BotMyst.Bot.Helpers;
using Newtonsoft.Json.Linq;

namespace BotMyst.Bot
{
    public class BotMyst
    {
        public static IConfiguration Configuration { get; set; }

        private DiscordSocketClient client;
        private CommandService commandService;
        private IServiceProvider services;

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

            commandService = new CommandService ();

            services = new ServiceCollection ()
                    .BuildServiceProvider ();

            await InstallCommands ();

            await client.LoginAsync (TokenType.Bot, Configuration ["discordToken"]);
            await client.SetGameAsync ($"{Configuration ["prefix"]}help botmyst.com");
            await client.StartAsync ();

            client.Log += Log;

            client.Ready += OnReady;

            await Task.Delay (-1);
        }

        private Task OnReady ()
        {
            foreach (var g in client.Guilds)
                BotMystAPI.GenerateOptions (g.Id);
                
            return Task.CompletedTask;
        }

        private Task Log (LogMessage arg)
        {
            System.Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task InstallCommands ()
        {
            client.MessageReceived += HandleCommand;
            await commandService.AddModulesAsync (Assembly.GetEntryAssembly ());
            BotMystAPI.SendModuleData (commandService);
        }

        private async Task HandleCommand (SocketMessage arg)
        {
            SocketUserMessage message = arg as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;

            if ((message.HasStringPrefix(Configuration["prefix"], ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos)) == false) return;

            CommandContext context = new CommandContext(client, message);
            CommandInfo executedCommand = commandService.Search(context, argPos).Commands[0].Command;

            CommandOptionsAttribute at = (CommandOptionsAttribute) executedCommand.Attributes.First(a => a.GetType() == typeof(CommandOptionsAttribute));
            var optionsJson = BotMystAPI.GetOptions (at.CommandOptionsType, context.Guild.Id);
            var options = JsonConvert.DeserializeObject (optionsJson);
            JObject jo = options as JObject;
            // The command is disabled
            if (((bool) jo ["enabled"]) == false)
                return;

            IResult result = await commandService.ExecuteAsync(context, argPos, services);
            if (result.IsSuccess == false)
            {
                if (result.Error == CommandError.UnknownCommand) return;
                if (result.Error == CommandError.BadArgCount)
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    eb.WithTitle("ERROR: Bad argument count");
                    eb.WithColor(Color.Red);
                    await context.Channel.SendMessageAsync(string.Empty, false, eb);
                }
                else
                {
                    EmbedBuilder eb = new EmbedBuilder();
                    eb.WithTitle("Error");
                    eb.WithDescription(result.ErrorReason);
                    eb.WithColor(Color.Red);
                    await context.Channel.SendMessageAsync(string.Empty, false, eb);
                }
            }

            if (((bool) jo ["deleteInvocationMessage"]) == false)
                await arg.DeleteAsync ();
        }
    }
}