using System;
using System.Diagnostics;
using System.Net.Http;

namespace Client.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();

            Console.WriteLine("Running...");

            while (true)
            {
                HttpResponseMessage response = client.GetAsync("https://localhost:5001/health").Result;

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
