using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Client.ConsoleApp
{
    public class AppSettings
    {
        public string Endpoint { get; set; }
    }

    public class Program
    {
        static AppSettings appSettings = new AppSettings();

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            ConfigurationBinder.Bind(configuration.GetSection("AppSettings"), appSettings);

            while (true)
            {
                try
                {
                    Uri uri = new Uri(appSettings.Endpoint);
                    var client = new HttpClient();

                    Console.Title = uri.ToString();

                    client.GetAsync(uri).Result.StatusCode.Should().Be(HttpStatusCode.OK);

                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Clear();
                    Console.WriteLine($"{DateTime.Now}\nOK");
                }
                catch (Exception e)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Clear();
                    Console.WriteLine($"{DateTime.Now}\n{e.Message}");
                }

                Thread.Sleep(1000);
            }
        }
    }
}
