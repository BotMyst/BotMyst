using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace BotMyst.Web
{
    public class Startup
    {
        private IConfiguration configuration;

        public Startup ()
        {
            configuration = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json")
                .Build ();
        }

        public void ConfigureServices (IServiceCollection services)
        {
            services.AddMvc ();

            services.AddAuthentication (options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Discord";
            })
            .AddCookie (options =>
            {
                options.Cookie = new CookieBuilder ()
                {
                    Name = "Discord",
                    Expiration = new TimeSpan (2, 0, 0, 0)
                };
            })
            .AddOAuth ("Discord", options =>
            {
                options.ClientId = configuration ["Discord:ClientId"];
                options.ClientSecret = configuration ["Discord:ClientSecret"];
                options.CallbackPath = new PathString ("/discord");

                options.AuthorizationEndpoint = "https://discordapp.com/api/oauth2/authorize";
                options.TokenEndpoint = "https://discordapp.com/api/oauth2/token";
                options.UserInformationEndpoint = "https://discordapp.com/api/users/@me";

                options.ClaimActions.MapJsonKey (ClaimTypes.NameIdentifier, "id", ClaimValueTypes.UInteger64);
                options.ClaimActions.MapJsonKey (ClaimTypes.Name, "username", ClaimValueTypes.String);
                options.ClaimActions.MapJsonKey ("urn:discord:discriminator", "discriminator", ClaimValueTypes.UInteger32);
                options.ClaimActions.MapJsonKey ("urn:discord:avatar", "avatar", ClaimValueTypes.String);
                options.ClaimActions.MapJsonKey ("urn:discord:verified", "verified", ClaimValueTypes.Boolean);

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue ("Bearer", context.AccessToken);

                        HttpResponseMessage response = await context.Backchannel.SendAsync (request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode ();
                    
                        JObject user = JObject.Parse (await response.Content.ReadAsStringAsync ());
                    
                        context.RunClaimActions (user);
                    }
                };

                options.Scope.Add ("identify");
                options.Scope.Add ("guilds");
                options.Scope.Add ("email");
            });
        }

        public void Configure (IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment ())
                app.UseDeveloperExceptionPage ();
            
            app.UseStaticFiles ();

            app.UseAuthentication ();

            app.UseMvc (routes =>
            {
                routes.MapRoute
                (
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
