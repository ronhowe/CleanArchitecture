using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Client.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Uri uri = new(args[0]);

            while (true)
            {
                HttpClient client = new();

                try
                {
                    client.GetAsync(uri).Result.StatusCode.Should().Be(HttpStatusCode.OK);

                    Refresh("OK", uri, ConsoleColor.DarkGreen);
                }
                catch (Exception e)
                {
                    Refresh(e.Message, uri, ConsoleColor.DarkRed);
                }
                finally
                {
                    client.Dispose();
                }

                Thread.Sleep(1000);
            }
        }

        private static void Refresh(string message, Uri uri, ConsoleColor color)
        {
            Console.BackgroundColor = color;
            Console.Clear();
            Console.WriteLine($"{DateTime.Now} - {uri} - {message}");
        }
    }
}
