using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;

while (true)
{
    try
    {
        const string uri = "https://app.ididevsecops.net";
        var client = new HttpClient();

        Console.Title = uri;

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
