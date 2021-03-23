using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Server.WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var settings = config.Build();

                        //var connectionString = settings.GetConnectionString("AzureAppConfiguration");

                        config.AddAzureAppConfiguration(options =>
                        {
                            options
                            //.Connect(connectionString)
                            //.Connect(new Uri(settings["AppConfig:Endpoint"]), new ManagedIdentityCredential())
                            .Connect(new Uri(settings["AppConfig:Endpoint"]), new DefaultAzureCredential(true))
                                   .ConfigureRefresh(refresh =>
                                   {
                                       refresh.Register("sentinel", refreshAll: true)
                                      .SetCacheExpiration(new TimeSpan(0, 0, 3));
                                   })

                                   .UseFeatureFlags(featureFlagOptions =>
                                   {
                                       featureFlagOptions.CacheExpirationInterval = new TimeSpan(0, 0, 3);
                                   });
                        });
                    }).UseStartup<Startup>();
                });
    }
}
