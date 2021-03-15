using System;
using System.Diagnostics;
using System.Net.Http;

namespace Client.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = false;

            Refresh(result);

            HttpClient client = new();

            while (true)
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync("https://localhost:5001/health").Result;

                    response.EnsureSuccessStatusCode();

                    result = true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
                finally
                {
                    Refresh(result);

                    result = false;
                }
            }

            static void Refresh(bool result)
            {
                Console.Clear();

                Console.BackgroundColor = result ? ConsoleColor.Green : ConsoleColor.Red;
            }
        }
    }
}
