using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

                        // https://csharp.christiannagel.com/2020/05/19/azureappconfiguration/
                        // dotnet user-secrets init
                        // dotnet user - secrets set ConnectionStrings:AzureAppConfiguration "the secret connection string"

                        // https://docs.microsoft.com/en-us/azure/azure-app-configuration/enable-dynamic-configuration-aspnet-core?tabs=core3x
                        var connectionString = settings.GetConnectionString("AzureAppConfiguration");

                        config.AddAzureAppConfiguration(options =>
                        {
                            options.Connect(connectionString)
                                   .ConfigureRefresh(refresh =>
                                   {
                                       refresh.Register("sentinel", refreshAll: true)
                                      .SetCacheExpiration(new TimeSpan(0, 0, 3));
                                   })

                                   // https://docs.microsoft.com/en-us/azure/azure-app-configuration/use-feature-flags-dotnet-core
                                   // https://docs.microsoft.com/en-us/azure/azure-app-configuration/quickstart-feature-flag-aspnet-core?tabs=core3x
                                   .UseFeatureFlags(featureFlagOptions =>
                                   {
                                       featureFlagOptions.CacheExpirationInterval = new TimeSpan(0, 0, 3);
                                   });
                        });
                    }).UseStartup<Startup>();
                });
    }
}
