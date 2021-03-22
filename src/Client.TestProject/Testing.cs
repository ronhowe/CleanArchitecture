using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Solution.Client.TestProject
{
    [SetUpFixture]
    public class Testing
    {
        internal static IConfigurationRoot Configuration;
        //private static string _unauthorizedToken;
        //private static string _authorizedToken;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            //_configuration = AuthConfig.ReadFromJsonFile("appsettings.json");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

           Configuration = builder.Build();

            //_unauthorizedToken = GetBearerTokenAsync().Result;
            //_authorizedToken = GetBearerTokenAsync().Result;
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }

        //public static async Task<string> GetBearerTokenAsync()
        //{
        //    IConfidentialClientApplication app;

        //    app = ConfidentialClientApplicationBuilder.Create(_configuration.ClientId)
        //        .WithClientSecret(_configuration.ClientSecret)
        //        .WithAuthority(_configuration.Authority)
        //        .Build();

        //    string[] ResourceIds = new string[] { _configuration.ResourceID };

        //    AuthenticationResult result = null;

        //    Stopwatch stopWatch = new Stopwatch();

        //    TimeSpan ts;

        //    string elapsed;

        //    stopWatch.Start();

        //    Trace.WriteLine($"request_start={DateTime.Now.ToString()}");
        //    result = await app.AcquireTokenForClient(ResourceIds).ExecuteAsync();
        //    Trace.WriteLine($"request_end={DateTime.Now.ToString()}");

        //    stopWatch.Stop();

        //    ts = stopWatch.Elapsed;

        //    elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

        //    Trace.WriteLine($"request_elapsed={elapsed}");

        //    Assert.IsFalse(string.IsNullOrEmpty(result.AccessToken));

        //    Trace.WriteLine($"token={result.AccessToken}");

        //    return result.AccessToken;
        //}
        //public static async Task<HttpResponseMessage> GetHttpResponseMessageHealthAddressAsAnonymous()
        //{
        //    return await GetHttpResponseMessage(new Uri(_configuration.GetAddress), null);
        //}

        //public static async Task<HttpResponseMessage> GetHttpResponseMessageThirdPartyAddressAsAnonymous()
        //{
        //    return await GetHttpResponseMessage(new Uri(_configuration.ThirdPartyAddress), null);
        //}

        //public static async Task<HttpResponseMessage> GetHttpResponseMessageBaseAddressAsAnonymous()
        //{
        //    return await GetHttpResponseMessage(new Uri(_configuration.GetAddress), null);
        //}

        //public static async Task<HttpResponseMessage> GetHttpResponseMessageBaseAddressAsDefaultUser()
        //{
        //    return await GetHttpResponseMessage(new Uri(_configuration.GetAddress), _unauthorizedToken);
        //}

        //public static async Task<HttpResponseMessage> GetHttpResponseMessageBaseAddressAsAdmin()
        //{
        //    return await GetHttpResponseMessage(new Uri(_configuration.GetAddress), _authorizedToken);
        //}

        public static async Task<HttpResponseMessage> GetHttpResponseMessageFromInternetEndpoint()
        {
            return await GetHttpResponseMessageAsAnonymous(new Uri(Configuration["InternetEndpoint"]));
        }

        public static async Task<HttpResponseMessage> GetHttpResponseMessageFromApplicationEndpoint()
        {
            return await GetHttpResponseMessageAsAnonymous(new Uri(Configuration["ApplicationEndpoint"]));
        }

        public static async Task<HttpResponseMessage> GetHttpResponseMessageFromFrontDoorEndpoint()
        {
            return await GetHttpResponseMessageAsAnonymous(new Uri(Configuration["FrontDoorEndpoint"]));
        }

        public static async Task<HttpResponseMessage> GetHttpResponseMessageFromApiManagementEndpoint()
        {
            return await GetHttpResponseMessageAsAnonymous(new Uri(Configuration["ApiManagementEndpoint"]));
        }

        public static async Task<HttpResponseMessage> GetHttpResponseMessageAsAnonymous(Uri uri)
        {
            return await GetHttpResponseMessage(uri, null);
        }

        public static async Task<HttpResponseMessage> GetHttpResponseMessage(Uri uri, string token)
        {
            var client = new HttpClient();

            if (token != null && token.Length > 0)
            {
                var defaultRequestHeaders = client.DefaultRequestHeaders;
                if (defaultRequestHeaders.Accept == null ||
                        !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            }

            Stopwatch stopWatch = new Stopwatch();

            TimeSpan ts;

            string elapsed;

            stopWatch.Start();

            Trace.WriteLine($"uri={uri}");

            Trace.WriteLine($"start={DateTime.Now.ToString()}");

            HttpResponseMessage response = await client.GetAsync(uri);

            Trace.WriteLine($"stop={DateTime.Now.ToString()}");

            stopWatch.Stop();

            ts = stopWatch.Elapsed;

            elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Trace.WriteLine($"elapsed={elapsed}");

            return response;
        }

        public static async Task ResetState()
        {
            await Task.Delay(0);
        }
    }
}
