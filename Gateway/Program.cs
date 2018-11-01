using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace ProxyTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseKestrel()
                   .UseCloudFoundryHosting()
                   .AddCloudFoundry()
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config
                            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                            .AddJsonFile("appsettings.json", true, true)
                            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                            .AddJsonFile("ocelot.json")
                            .AddEnvironmentVariables();
                    })
                    .UseIISIntegration()
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.ClearProviders();
                        logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        logging.AddConsole();
                    })
                   ;
    }
}
