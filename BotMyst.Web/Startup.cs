using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Newtonsoft.Json.Linq;

using BotMyst.Web.Authentication;
using BotMyst.Web.DatabaseContexts;

namespace BotMyst.Web
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public Startup ()
        {
            Configuration = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddJsonFile ("appsettings.json")
                .Build ();
        }

        public void ConfigureServices (IServiceCollection services)
        {
            services.AddDbContext<ModuleDescriptionsContext> (options => options.UseSqlite (Configuration.GetConnectionString ("ModuleDescriptions")));

            services.AddMvc ();

            services.AddAuthentication (options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "Discord";
            })
            .AddJwtBearer (options =>
            {
                options.Authority = Configuration ["Auth0:Domain"];
                options.Audience = Configuration ["Auth0:ApiIdentifier"];
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
                options.ClientId = Configuration ["Discord:ClientId"];
                options.ClientSecret = Configuration ["Discord:ClientSecret"];
                options.CallbackPath = new PathString ("/discord");

                options.AuthorizationEndpoint = "https://discordapp.com/api/oauth2/authorize";
                options.TokenEndpoint = "https://discordapp.com/api/oauth2/token";
                options.UserInformationEndpoint = "https://discordapp.com/api/users/@me";

                options.ClaimActions.MapJsonKey (ClaimTypes.NameIdentifier, "id", ClaimValueTypes.UInteger64);
                options.ClaimActions.MapJsonKey (ClaimTypes.Name, "username", ClaimValueTypes.String);
                options.ClaimActions.MapJsonKey ("urn:discord:discriminator", "discriminator", ClaimValueTypes.UInteger32);
                options.ClaimActions.MapJsonKey ("urn:discord:avatar", "avatar", ClaimValueTypes.String);
                options.ClaimActions.MapJsonKey ("urn:discord:verified", "verified", ClaimValueTypes.Boolean);

                options.SaveTokens = true;

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

            services.AddAuthorization (options =>
            {
                options.AddPolicy ("master", policy => policy.Requirements.Add (new HasScopeRequirement ("master", Configuration ["Auth0:Domain"])));
            });

            services.AddSingleton<IAuthorizationHandler, HasScopeHandler> ();
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
