using System;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace BotMyst.Web
{
    public class Program
    {
        public static void Main (string [] args)
        {
            IConfiguration configuration = new ConfigurationBuilder ()
                .SetBasePath (Directory.GetCurrentDirectory ())
                .AddEnvironmentVariables ()
                .AddJsonFile ("certificate.json", optional: true, reloadOnChange: true)
                .AddJsonFile ($"certificate.{Environment.GetEnvironmentVariable ("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .Build ();

            string certificateFileName = configuration ["CertificateSettings:FileName"];
            string certificatePassword = configuration ["CertificateSettings:Password"];

            X509Certificate2 certificate = new X509Certificate2 (certificateFileName, certificatePassword);

            // IWebHost host = new WebHostBuilder ()
            //     .UseKestrel (options =>
            //     {
            //         options.AddServerHeader = false;
            //         options.Listen (IPAddress.Loopback, 5001, listenOptions =>
            //         {
            //             listenOptions.UseHttps (certificate);
            //         });
            //     })
            //     .UseConfiguration (configuration)
            //     .UseContentRoot (Directory.GetCurrentDirectory ())
            //     .UseStartup<Startup> ()
            //     .UseUrls ("http://localhost:5000", "https://localhost:50001")
            //     .Build ();

            IWebHost host = WebHost.CreateDefaultBuilder (args)
                .UseKestrel (options =>
                {
                    options.AddServerHeader = false;
                    options.Listen (IPAddress.Loopback, 5001, listenOptions =>
                    {
                        listenOptions.UseHttps (certificate);
                    });
                })
                .UseStartup<Startup> ()
                .Build ();

            host.Run ();
        }
    }
}
