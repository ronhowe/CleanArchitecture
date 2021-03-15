using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace POST
{
    [TestClass]
    public class POST
    {
        [TestMethod]
        public void Post()
        {
            Debug.WriteLine("POST");
            Debug.WriteLine(DateTime.Now);
            Assert.IsTrue(true);
            PostBoot();
        }

        private static void PostBoot()
        {
            HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = client.GetAsync("https://localhost:5001/api/WeatherForecast").Result;

                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
