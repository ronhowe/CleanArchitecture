using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace Client.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri uri = new Uri("https://localhost:5001/health");

            Console.Title = uri.ToString();

            Refresh(ConsoleColor.DarkYellow);

            HttpClient client = new();

            while (true)
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(uri).Result;

                    response.EnsureSuccessStatusCode();

                    Refresh(ConsoleColor.DarkGreen);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);

                    Refresh(ConsoleColor.DarkRed);
                }

                Thread.Sleep(1000);
            }

        }

        private static void Refresh(ConsoleColor color)
        {
            Console.BackgroundColor = color;

            Console.Clear();
        }
    }
}
