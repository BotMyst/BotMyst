using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.Cookies;

using Newtonsoft.Json.Linq;

namespace BotMyst.Web
{
    public class Startup
    {
        public static IConfiguration Configuration;

        public Startup (IConfiguration configuration)
        {
            Configuration = configuration;
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
                    Name = "DiscordCookie",
                    Expiration = new TimeSpan (2, 0, 0, 0)
                };
            })
            .AddOAuth ("Discord", options =>
            {
                options.ClientId = Configuration ["Discord:ClientId"];
                options.ClientSecret = Configuration ["Discord:ClientSecret"];
                options.CallbackPath = new PathString("/signin-discord");

                options.AuthorizationEndpoint = "https://discordapp.com/api/oauth2/authorize";
                options.TokenEndpoint = "https://discordapp.com/api/oauth2/token";
                options.UserInformationEndpoint = "https://discordapp.com/api/users/@me";

                options.ClaimActions.MapJsonKey (ClaimTypes.NameIdentifier, "id", ClaimValueTypes.UInteger64);
                options.ClaimActions.MapJsonKey (ClaimTypes.Name, "username", ClaimValueTypes.String);
                options.ClaimActions.MapJsonKey (ClaimTypes.Email, "email", ClaimValueTypes.Email);
                options.ClaimActions.MapJsonKey ("urn:discord:discriminator", "discriminator", ClaimValueTypes.UInteger32);
                options.ClaimActions.MapJsonKey ("urn:discord:avatar", "avatar", ClaimValueTypes.String);
                options.ClaimActions.MapJsonKey ("urn:discord:verified", "verified", ClaimValueTypes.Boolean);

                options.SaveTokens = true;

                options.Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage (HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));
                        request.Headers.Authorization = new AuthenticationHeaderValue ("Bearer", context.AccessToken);

                        var response = await context.Backchannel.SendAsync (request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode ();

                        var user = JObject.Parse (await response.Content.ReadAsStringAsync ());

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
            {
                app.UseDeveloperExceptionPage ();
            }
            else
            {
                app.UseExceptionHandler ("/Home/Error");
            }

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
