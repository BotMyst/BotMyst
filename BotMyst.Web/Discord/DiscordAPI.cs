using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Newtonsoft.Json;

using BotMyst.Web.Models;
using BotMyst.Web.Discord.Models;

namespace BotMyst.Web.Discord
{
    public static class DiscordAPI
    {
        public const string ApiUrl = "https://discordapp.com/api/";

        /// <summary>
        /// Get all guilds in which the current user is in.
        /// </summary>
        public static async Task<List<DiscordGuild>> GetUserGuildsAsync (HttpContext httpContext)
        {
            string token = await httpContext.GetTokenAsync (CookieAuthenticationDefaults.AuthenticationScheme, "access_token");
        
            return await GetUserDiscordList<DiscordGuild> (token, "users/@me/guilds");
        }

        /// <summary>
        /// Get a guild in which the current user is in.
        /// </summary>
        public static async Task<DiscordGuild> GetUserGuildAsync (ulong guildId, HttpContext httpContext)
        {
            string token = await httpContext.GetTokenAsync (CookieAuthenticationDefaults.AuthenticationScheme, "access_token");
        
            return (await GetUserGuildsAsync (httpContext)).FirstOrDefault (g => g.Id == guildId);
        }

        /// <summary>
        /// Returns a BotMyst guild which has a member which indicates whether BotMyst is in that guild.
        /// </summary>
        public static async Task<BotMystGuild> GetBotMystGuildAsync (ulong guildId, HttpContext httpContext)
        {
            BotMystGuild botMystGuild = new BotMystGuild ();

            botMystGuild.Guild = await GetBotGuildAsync (guildId);

            if (botMystGuild.Guild == null)
            {
                botMystGuild.Guild = await GetUserGuildAsync (guildId, httpContext);
                botMystGuild.HasBotMyst = false;
            }
            else
            {
                botMystGuild.HasBotMyst = true;
            }

            return botMystGuild;
        }

        /// <summary>
        /// Get all guilds in which the current user has admin permissions in. Includes a member that indicates whether BotMyst is in the server.
        /// </summary>
        public static async Task<List<BotMystGuild>> GetBotMystGuildsAsync (HttpContext httpContext)
        {
            List<DiscordGuild> guilds = (await GetUserGuildsAsync (httpContext)).WherePermissions (8).ToList ();

            List<BotMystGuild> botMystGuilds = new List<BotMystGuild> ();

            foreach (var guild in guilds)
            {
                botMystGuilds.Add (new BotMystGuild
                {
                    Guild = guild,
                    HasBotMyst = await GetBotGuildAsync (guild.Id) != null
                });
            }

            return botMystGuilds;
        }

        /// <summary>
        /// Get a guild using an id. This guild should be accessible by BotMyst, otherwise it's null.
        /// </summary>
        public static async Task<DiscordGuild> GetBotGuildAsync (ulong guildId) =>
            await GetBotDiscordObject<DiscordGuild> ($"guilds/{guildId}");

        /// <summary>
        /// Returns a list of Discord objects which are accessed by the /users/@me/ url.
        /// </summary>
        private static async Task<List<T>> GetUserDiscordList<T> (string accessToken, string requestUri)
        {
            List<T> result = new List<T> ();

            HttpClient httpClient = new HttpClient ();

            httpClient.BaseAddress = new Uri (ApiUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", accessToken);  
            httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));

            HttpResponseMessage response = await httpClient.GetAsync (requestUri);  
            if (response.IsSuccessStatusCode)          
            {
                string json = await response.Content.ReadAsStringAsync ();
                result = JsonConvert.DeserializeObject<List<T>> (json);
            }
            response.EnsureSuccessStatusCode ();

            return result;
        }

        /// <summary>
        /// Returns a Discord object which is accessed by the /users/@me/ url.
        /// </summary>
        private static async Task<T> GetUserDiscordObject<T> (string accessToken, string requestUri)
        {
            HttpClient httpClient = new HttpClient ();

            httpClient.BaseAddress = new Uri (ApiUrl);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", accessToken);  
            httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));

            HttpResponseMessage response = await httpClient.GetAsync (requestUri);  
            if (response.IsSuccessStatusCode)          
            {
                string json = await response.Content.ReadAsStringAsync ();
                System.Console.WriteLine(json);
                return JsonConvert.DeserializeObject<T> (json);
            }
            else
            {
                return default (T);
            }
        }

        /// <summary>
        /// Returns a  Discord object which need bot authorization to access.
        /// </summary>
        private static async Task<T> GetBotDiscordObject<T> (string requestUri)
        {
            HttpClient httpClient = new HttpClient ();
            httpClient.BaseAddress = new Uri (ApiUrl);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bot", Startup.Configuration ["Discord:BotToken"]);   
            httpClient.DefaultRequestHeaders.Accept.Clear ();         
            httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
            httpClient.DefaultRequestHeaders.Connection.Clear ();
            httpClient.DefaultRequestHeaders.Connection.Add (HttpMethod.Get.ToString ().ToUpper ());

            HttpResponseMessage response = await httpClient.GetAsync (requestUri);  
            if (response.IsSuccessStatusCode)          
            {
                string json = await response.Content.ReadAsStringAsync ();
                return JsonConvert.DeserializeObject<T> (json);
            }
            else
            {
                return default (T);
            }
        }
    }
}
