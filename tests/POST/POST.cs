using Microsoft.Identity.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Timers;

namespace POST
{
    [TestClass]
    public class POST
    {
        [TestMethod]
        public void Boot()
        {
            ///////////////////////////////////////////////////////
            #region POST

            Trace.WriteLine($"post={DateTime.Now.ToString()}");

            #endregion POST
            ///////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////
            #region CONFIGURATION

            AuthConfig config = AuthConfig.ReadFromJsonFile("appsettings.json");

            Trace.WriteLine($"config.Authority={config.Authority}");
            Trace.WriteLine($"config.BaseAddress={config.BaseAddress}");
            Trace.WriteLine($"config.ClientId={config.ClientId}");
            Trace.WriteLine($"config.ClientSecret={config.ClientSecret}");
            Trace.WriteLine($"config.ResourceID={config.ResourceID}");
            Trace.WriteLine($"config.TenantId={config.TenantId}");

            Uri uri = new Uri(config.BaseAddress);
            HttpClient anonymousHttpClient = new HttpClient();
            Stopwatch stopWatch = new Stopwatch();
            TimeSpan ts;
            string elapsed;

            #endregion CONFIGURATION
            ///////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////
            #region UNAUTHENTICATED

            SetTimer();

            stopWatch.Start();

            Trace.WriteLine($"anonymous_request_start={DateTime.Now.ToString()}");
            HttpResponseMessage response = anonymousHttpClient.GetAsync(uri).Result;
            Trace.WriteLine($"anonymous_request_stop={DateTime.Now.ToString()}");

            stopWatch.Stop();

            aTimer.Stop();

            aTimer.Dispose();

            ts = stopWatch.Elapsed;

            elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Trace.WriteLine($"anonymous_request_elapsed={elapsed}");

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Unauthorized);

            #endregion UNAUTHENTICATED
            ///////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////
            #region AUTHENTICATE

            IConfidentialClientApplication app;

            app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                .WithClientSecret(config.ClientSecret)
                .WithAuthority(new Uri(config.Authority))
                .Build();

            string[] ResourceIds = new string[] { config.ResourceID };

            AuthenticationResult result = null;

            SetTimer();

            stopWatch.Start();

            Trace.WriteLine($"_request_start={DateTime.Now.ToString()}");
            result = app.AcquireTokenForClient(ResourceIds).ExecuteAsync().Result;
            Trace.WriteLine($"token_request_stop={DateTime.Now.ToString()}");

            stopWatch.Stop();

            aTimer.Stop();

            aTimer.Dispose();

            ts = stopWatch.Elapsed;

            elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Trace.WriteLine($"token_request_elapsed={elapsed}");

            Assert.IsFalse(string.IsNullOrEmpty(result.AccessToken));

            Trace.WriteLine($"token={result.AccessToken}");

            #endregion AUTHENTICATE
            ///////////////////////////////////////////////////////

            ///////////////////////////////////////////////////////
            #region AUTENTICATED

            var authenticatedHttpClient = new HttpClient();
            var defaultRequestHeaders = authenticatedHttpClient.DefaultRequestHeaders;

            if (defaultRequestHeaders.Accept == null ||
                    !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
            {
                authenticatedHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);

            SetTimer();

            stopWatch.Start();

            Trace.WriteLine($"authenticated_request_start={DateTime.Now.ToString()}");
            response = authenticatedHttpClient.GetAsync(config.BaseAddress).Result;
            Trace.WriteLine($"authenticated_request_stop={DateTime.Now.ToString()}");

            stopWatch.Stop();

            aTimer.Stop();

            aTimer.Dispose();

            ts = stopWatch.Elapsed;

            elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Trace.WriteLine($"authenticated_request_elapsed={elapsed}");

            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);

            string content = response.Content.ReadAsStringAsync().Result;
            Trace.WriteLine($"content={content}");

            #endregion AUTENTICATED
            ///////////////////////////////////////////////////////
        }

        private static System.Timers.Timer aTimer;

        private static void SetTimer()
        {
            aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Trace.WriteLine($"timer={DateTime.Now.ToString()}");
            //Trace.WriteLine("OnTimedEvent @ {0:HH:mm:ss.fff}", e.SignalTime);
        }
    }
}
