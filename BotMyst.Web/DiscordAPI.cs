using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using Newtonsoft.Json;

using BotMyst.Web.Models;
using System.Collections.Generic;

namespace BotMyst.Web
{
    public class DiscordAPI
    {
        private static HttpClient httpClient;

        public DiscordAPI ()
        {
            httpClient = new HttpClient ();
            httpClient.BaseAddress = new Uri ("https://discordapp.com/api/");
        }

        public async Task<List<DiscordGuildModel>> GetUserGuildsAsync (HttpContext httpContext)
        {
            List<DiscordGuildModel> guilds = new List<DiscordGuildModel> ();

            string token = await httpContext.GetTokenAsync (CookieAuthenticationDefaults.AuthenticationScheme, "access_token");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", token);   
            httpClient.DefaultRequestHeaders.Accept.Clear ();         
            httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
            httpClient.DefaultRequestHeaders.Connection.Clear ();
            httpClient.DefaultRequestHeaders.Connection.Add ("GET");

            HttpResponseMessage response = await httpClient.GetAsync ("users/@me/guilds");  
            if (response.IsSuccessStatusCode)          
            {
                string json = await response.Content.ReadAsStringAsync ();
                guilds = JsonConvert.DeserializeObject<List<DiscordGuildModel>> (json);
            }
            response.EnsureSuccessStatusCode ();

            return guilds;
        }

        public async Task<DiscordGuildModel> GetGuildAsync (string guildId)
        {
            httpClient.DefaultRequestHeaders.Clear ();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bot", Startup.Configuration ["Discord:BotToken"]);  
            httpClient.DefaultRequestHeaders.Accept.Clear ();
            httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
            httpClient.DefaultRequestHeaders.Connection.Clear ();
            httpClient.DefaultRequestHeaders.Connection.Add ("GET");

            HttpResponseMessage response = await httpClient.GetAsync ($"guilds/{guildId}");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync ();
                return JsonConvert.DeserializeObject<DiscordGuildModel> (json);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<DiscordRoleModel>> GetGuildRolesAsync (string guildId)
        {
            httpClient.DefaultRequestHeaders.Clear ();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bot", Startup.Configuration ["Discord:BotToken"]);  
            httpClient.DefaultRequestHeaders.Accept.Clear ();
            httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
            httpClient.DefaultRequestHeaders.Connection.Clear ();
            httpClient.DefaultRequestHeaders.Connection.Add ("GET");

            HttpResponseMessage response = await httpClient.GetAsync ($"guilds/{guildId}/roles");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync ();
                return JsonConvert.DeserializeObject<List<DiscordRoleModel>> (json);
            }
            else
            {
                return null;
            }
        }

        public async Task<List<DiscordChannelModel>> GetGuildChannelsAsync (string guildId)
        {
            httpClient.DefaultRequestHeaders.Clear ();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bot", Startup.Configuration ["Discord:BotToken"]);  
            httpClient.DefaultRequestHeaders.Accept.Clear ();
            httpClient.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
            httpClient.DefaultRequestHeaders.Connection.Clear ();
            httpClient.DefaultRequestHeaders.Connection.Add ("GET");

            HttpResponseMessage response = await httpClient.GetAsync ($"guilds/{guildId}/channels");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync ();
                return JsonConvert.DeserializeObject<List<DiscordChannelModel>> (json);
            }
            else
            {
                return null;
            }
        } 
    }
}
