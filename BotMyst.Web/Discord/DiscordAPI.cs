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
        /// Get all guilds in which the current user has admin permissions in. Includes a member that indicates whether BotMyst is in the server.
        /// </summary>
        public static async Task<List<BotMystGuild>> GetBotMystGuildsAsync (HttpContext httpContext)
        {
            List<DiscordGuild> guilds = (await DiscordAPI.GetUserGuildsAsync (httpContext)).WherePermissions (8).ToList ();

            List<BotMystGuild> botMystGuilds = new List<BotMystGuild> ();

            foreach (var guild in guilds)
            {
                botMystGuilds.Add (new BotMystGuild
                {
                    Guild = guild,
                    HasBotMyst = await DiscordAPI.GetGuildAsync (guild.Id) != null
                });
            }

            return botMystGuilds;
        }

        /// <summary>
        /// Get a guild using an id.
        /// </summary>
        public static async Task<DiscordGuild> GetGuildAsync (ulong guildId) =>
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
                System.Console.WriteLine(json);
                result = JsonConvert.DeserializeObject<List<T>> (json);
            }
            response.EnsureSuccessStatusCode ();

            return result;
        }

        /// <summary>
        /// Returns a list of Discord objects which need bot authorization to access.
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
