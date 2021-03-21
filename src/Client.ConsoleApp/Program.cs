using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Client.ConsoleApp
{
    public class AppSettings
    {
        public string Endpoint { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Uri uri = new Uri(args[0]);
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
