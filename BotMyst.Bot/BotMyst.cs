using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using BotMyst.Bot.Models;
using BotMyst.Bot.Helpers;

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
            Console.WriteLine(arg);
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

            if ((message.HasStringPrefix (Configuration["prefix"], ref argPos) || message.HasMentionPrefix (client.CurrentUser, ref argPos)) == false) return;

            CommandContext context = new CommandContext (client, message);
            CommandInfo executedCommand = commandService.Search (context, argPos).Commands [0].Command;

            CommandOptionsAttribute at = (CommandOptionsAttribute) executedCommand.Attributes.First (a => a.GetType () == typeof (CommandOptionsAttribute));
            var optionsJson = BotMystAPI.GetOptions (at.CommandOptionsType, context.Guild.Id);
            var options = JsonConvert.DeserializeObject (optionsJson) as JObject;

            // The command is disabled
            if (((bool) options ["enabled"]) == false)
                return;

            // Delete the invocation message
            if (((bool) options ["deleteInvocationMessage"]) == true)
                await arg.DeleteAsync ();

            string [] whitelistRoles = null;
            string [] blacklistRoles = null;

            string [] whitelistChannels = null;
            string [] blacklistChannels = null;

            if (string.IsNullOrEmpty ((string) options ["roleWhitelist"]) == false)
                whitelistRoles = ((string) options ["roleWhitelist"]).Trim ().Split (",");
            if (string.IsNullOrEmpty ((string) options ["roleBlacklist"]) == false)
                blacklistRoles = ((string) options ["roleBlacklist"]).Trim ().Split (",");

            if (string.IsNullOrEmpty ((string) options ["channelWhitelist"]) == false)
                whitelistChannels = ((string) options ["channelWhitelist"]).Trim ().Split (",");
            if (string.IsNullOrEmpty ((string) options ["channelBlacklist"]) == false)
                blacklistChannels = ((string) options ["channelBlacklist"]).Trim ().Split (",");

            SocketGuildUser guildUser = (SocketGuildUser) context.User;

            bool canRun = BotMystHelpers.CheckWhitelistBlacklistRoles (guildUser, whitelistRoles, blacklistRoles);
            canRun = BotMystHelpers.CheckWhitelistBlacklistChannels (context.Channel.Name, whitelistChannels, blacklistChannels);

            if (canRun == false)
            {
                EmbedBuilder eb = new EmbedBuilder();
                eb.WithTitle("ERROR: Insufficient permission");
                eb.WithDescription ("You don't have the permission to use this command.");
                eb.WithColor(Color.Orange);
                await context.Channel.SendMessageAsync(string.Empty, false, eb);
                return;
            }

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
        }
    }
}