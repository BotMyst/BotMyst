using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using AngleSharp;

using BotMyst.Data;

using Module = BotMyst.Data.Module;
using AngleSharp.Parser.Html;
using AngleSharp.Dom.Html;
using AngleSharp.Dom;
using System.Net;

namespace BotMyst
{
    public class BotMyst
    {
        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;

        public static readonly string CommandPrefix = ">";

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

            await InstallCommands ();

            await client.LoginAsync (TokenType.Bot, BotMystConfig.Token);
            await client.SetGameAsync (">help");
            await client.StartAsync ();

            Console.WriteLine ("BotMyst is ready.");

            await Task.Delay (-1);
        }

        private async Task RegisterModulesOnServer (IEnumerable<ModuleInfo> modules)
        {
            Uri baseUri = new Uri ("http://localhost:5000");

            CookieContainer cookieContainer = new CookieContainer ();
            HttpClientHandler handler = new HttpClientHandler ();
            handler.CookieContainer = cookieContainer;

            HttpClient getClient = new HttpClient (handler);
            getClient.BaseAddress = baseUri;
            getClient.DefaultRequestHeaders.Connection.Add ("GET");
            

            CookieContainer postCookieContainer = new CookieContainer ();
            HttpClientHandler postHandler = new HttpClientHandler ();
            postHandler.CookieContainer = postCookieContainer;

            HttpClient postClient = new HttpClient (postHandler);
            postClient.BaseAddress = baseUri;
            postClient.DefaultRequestHeaders.Connection.Add ("POST");
        
            HtmlParser parser = new HtmlParser ();

            for (int i = 0; i < modules.Count (); i++)
            {
                ModuleInfo m = modules.ElementAt (i);
                if (m.Name == "HelpCommand")
                    continue;

                Module module = new Module
                {
                    Name = m.Name,
                    Description = m.Summary,
                    Enabled = true,
                    Options = ""
                };

                string json = JsonConvert.SerializeObject (module);

                HttpResponseMessage getMsg = await getClient.GetAsync ("Modules/Create");
                string html = await (getMsg.Content.ReadAsStringAsync ());

                IHtmlDocument doc = await parser.ParseAsync (html);

                IElement tokenInput = doc.QuerySelector ("input[name=__RequestVerificationToken]");
                string token = tokenInput.Attributes ["value"].Value;

                IEnumerable<Cookie> responseCookies = cookieContainer.GetCookies (new Uri ("http://localhost:5000/Modules/Create")).Cast<Cookie> ();
                string cookie = responseCookies.FirstOrDefault (c => c.Name == ".AspNetCore.Antiforgery.YbN9yTuP6nI").Value;

                postClient.DefaultRequestHeaders.Clear ();
                postClient.DefaultRequestHeaders.Add ("RequestVerificationToken", token);
                postCookieContainer.SetCookies (new Uri (baseUri, "Modules/Create"), $".AspNetCore.Antiforgery.YbN9yTuP6nI={cookie}");
                HttpResponseMessage postResponse = await postClient.PostAsync (new Uri (baseUri, "Modules/Create"), new StringContent (json, Encoding.UTF8, "application/json"));
            }
        }

        private async Task InstallCommands ()
        {
            client.MessageReceived += HandleCommand;
            IEnumerable<ModuleInfo> modules = await commands.AddModulesAsync (Assembly.GetEntryAssembly ());            

            await RegisterModulesOnServer (modules);
        }

        private async Task HandleCommand (SocketMessage msg)
        {
            SocketUserMessage message = msg as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;

            if ((message.HasStringPrefix (CommandPrefix, ref argPos) || message.HasMentionPrefix (client.CurrentUser, ref argPos)) == false) return;

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
                if (result.Error == CommandError.BadArgCount)
                {
                    EmbedBuilder eb = new EmbedBuilder ();
                    eb.WithTitle ("ERROR: Bad argument count");
                    eb.WithColor (Color.Red);
                    await context.Channel.SendMessageAsync (string.Empty, false, eb);
                    await commands.ExecuteAsync (context, $"command {executedCommand.Name}");
                }
                else
                {
                    EmbedBuilder eb = new EmbedBuilder ();
                    eb.WithTitle ("Error");
                    eb.WithDescription (result.ErrorReason);
                    eb.WithColor (Color.Red);
                    await context.Channel.SendMessageAsync (string.Empty, false, eb);
                }
            }
        }
    }
}