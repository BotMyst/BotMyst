using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
