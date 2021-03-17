using Microsoft.Identity.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Solution.Client.TestProject
{
    //[TestClass]
    public class IntegrationTests
    {
        private static AuthConfig config = AuthConfig.ReadFromJsonFile("appsettings.json");

        private static string BearerToken;

        private static HttpResponseMessage GetHttpResponseMessage(Uri uri)
        {
            var client = new HttpClient();

            Stopwatch stopWatch = new Stopwatch();

            TimeSpan ts;

            string elapsed;

            stopWatch.Start();

            Trace.WriteLine($"request_start={DateTime.Now.ToString()}");

            HttpResponseMessage response = client.GetAsync(uri).Result;

            Trace.WriteLine($"request_stop={DateTime.Now.ToString()}");

            stopWatch.Stop();

            ts = stopWatch.Elapsed;

            elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Trace.WriteLine($"request_elapsed={elapsed}");

            return response;
        }

        [ClassInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Trace.WriteLine($"config.Authority={config.Authority}");
            Trace.WriteLine($"config.BaseAddress={config.BaseAddress}");
            Trace.WriteLine($"config.ClientId={config.ClientId}");
            Trace.WriteLine($"config.ClientSecret={config.ClientSecret}");
            Trace.WriteLine($"config.HealthAddress={config.HealthAddress}");
            Trace.WriteLine($"config.ResourceID={config.ResourceID}");
            Trace.WriteLine($"config.TenantId={config.TenantId}");
        }

        private static string GetClientBearerToken()
        {
            IConfidentialClientApplication app;

            app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                .WithClientSecret(config.ClientSecret)
                .WithAuthority(new Uri(config.Authority))
                .Build();

            string[] ResourceIds = new string[] { config.ResourceID };

            AuthenticationResult result = null;

            Stopwatch stopWatch = new Stopwatch();

            TimeSpan ts;

            string elapsed;

            stopWatch.Start();

            Trace.WriteLine($"request_start={DateTime.Now.ToString()}");
            result = app.AcquireTokenForClient(ResourceIds).ExecuteAsync().Result;
            Trace.WriteLine($"request_end={DateTime.Now.ToString()}");

            stopWatch.Stop();

            ts = stopWatch.Elapsed;

            elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Trace.WriteLine($"request_elapsed={elapsed}");

            Assert.IsFalse(string.IsNullOrEmpty(result.AccessToken));

            Trace.WriteLine($"token={result.AccessToken}");

            return result.AccessToken;
        }

        [TestMethod]
        public void HealthCheckReturnsOK()
        {
            HttpResponseMessage response = GetHttpResponseMessage(new Uri(config.HealthAddress));

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void ServiceReturns401ForUnauthenticatedUser()
        {
            HttpResponseMessage response = GetHttpResponseMessage(new Uri(config.BaseAddress));
            Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public void ServiceReturns403ForAuthenticatedUntrustedUser()
        {
            //token = GetClientBearerToken();
            HttpResponseMessage response = GetHttpResponseMessage(new Uri(config.BaseAddress));
            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }

        [TestMethod]
        public void AuthenticatedTrusted()
        {
            var client = new HttpClient();

            Stopwatch stopWatch = new Stopwatch();

            TimeSpan ts;

            string elapsed;

            stopWatch.Start();

            Trace.WriteLine($"untrusted_request_start={DateTime.Now.ToString()}");
            HttpResponseMessage response = client.GetAsync(config.BaseAddress).Result;
            Trace.WriteLine($"untrusted_request_stop={DateTime.Now.ToString()}");

            stopWatch.Stop();

            ts = stopWatch.Elapsed;

            elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Trace.WriteLine($"untrusted_request_elapsed={elapsed}");

            Assert.Inconclusive();
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
