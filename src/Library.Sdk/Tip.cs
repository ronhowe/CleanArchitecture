using Microsoft.Identity.Client;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Timers;

namespace Library.Sdk
{
    public static class Tip
    {
        public static void Run()
        {
            ///////////////////////////////////////////////////////
            #region CONFIGURATION

            AuthConfig config = AuthConfig.ReadFromJsonFile("appsettings.json");

            Trace.WriteLine($"config.Authority={config.Authority}");
            Trace.WriteLine($"config.BaseAddress={config.BaseAddress}");
            Trace.WriteLine($"config.ClientId={config.ClientId}");
            Trace.WriteLine($"config.ClientSecret={config.ClientSecret}");
            Trace.WriteLine($"config.HealthAddress={config.HealthAddress}");
            Trace.WriteLine($"config.ResourceID={config.ResourceID}");
            Trace.WriteLine($"config.TenantId={config.TenantId}");

            Uri uri = new Uri(config.BaseAddress);
            Uri uri2 = new Uri(config.HealthAddress);
            Stopwatch stopWatch = new Stopwatch();
            HttpResponseMessage response;
            TimeSpan ts;
            string elapsed;
            SetTimer();
            aTimer.Start();

            #endregion CONFIGURATION
            ///////////////////////////////////////////////////////

            try
            {
                ///////////////////////////////////////////////////////
                #region AUTHENTICATE

                IConfidentialClientApplication app;

                app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                    .WithClientSecret(config.ClientSecret)
                    .WithAuthority(new Uri(config.Authority))
                    .Build();

                string[] ResourceIds = new string[] { config.ResourceID };

                AuthenticationResult result = null;

                stopWatch.Start();

                Trace.WriteLine($"_request_start={DateTime.Now.ToString()}");
                result = app.AcquireTokenForClient(ResourceIds).ExecuteAsync().Result;
                Trace.WriteLine($"token_request_stop={DateTime.Now.ToString()}");

                stopWatch.Stop();

                ts = stopWatch.Elapsed;

                elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                Trace.WriteLine($"token_request_elapsed={elapsed}");

                //Assert.IsFalse(string.IsNullOrEmpty(result.AccessToken), "AUTHENTICATE TEST FAILURE");

                Trace.WriteLine($"token={result.AccessToken}");

                #endregion AUTHENTICATE
                ///////////////////////////////////////////////////////

                ///////////////////////////////////////////////////////
                #region ANONYMOUS

                var anonymousHttpClient = new HttpClient();

                stopWatch.Start();

                Trace.WriteLine($"anonymous_request_start={DateTime.Now.ToString()}");
                response = anonymousHttpClient.GetAsync(uri2).Result;
                Trace.WriteLine($"anonymous_request_stop={DateTime.Now.ToString()}");

                stopWatch.Stop();

                ts = stopWatch.Elapsed;

                elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                Trace.WriteLine($"anonymous_request_elapsed={elapsed}");

                //Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "ANONYMOUS TEST FAILURE");

                #endregion ANONYMOUS
                ///////////////////////////////////////////////////////

                ///////////////////////////////////////////////////////
                #region UNAUTHENTICATED

                var unauthenticatedHttpClient = new HttpClient();

                stopWatch.Start();

                Trace.WriteLine($"unauthenticated_request_start={DateTime.Now.ToString()}");
                response = unauthenticatedHttpClient.GetAsync(uri).Result;
                Trace.WriteLine($"unauthenticated_request_stop={DateTime.Now.ToString()}");

                stopWatch.Stop();

                ts = stopWatch.Elapsed;

                elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                Trace.WriteLine($"unauthenticated_request_elapsed={elapsed}");

                //Assert.AreEqual(System.Net.HttpStatusCode.Unauthorized, response.StatusCode, "UNAUTHENTICATED TEST FAILURE");

                #endregion UNAUTHENTICATED
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

                stopWatch.Start();

                Trace.WriteLine($"authenticated_request_start={DateTime.Now.ToString()}");
                response = authenticatedHttpClient.GetAsync(config.BaseAddress).Result;
                Trace.WriteLine($"authenticated_request_stop={DateTime.Now.ToString()}");

                stopWatch.Stop();

                ts = stopWatch.Elapsed;

                elapsed = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                Trace.WriteLine($"authenticated_request_elapsed={elapsed}");

                //Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "AUTENTICATED TEST FAILURE");

                string content = response.Content.ReadAsStringAsync().Result;
                Trace.WriteLine($"content={content}");

                #endregion AUTENTICATED
                ///////////////////////////////////////////////////////

                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.Clear();
                Console.WriteLine($"{DateTime.Now.ToString()}\nOK");

            }
            catch (Exception e)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.Clear();
                Console.WriteLine($"{DateTime.Now.ToString()}\n{e.Message}");
                Trace.TraceError($"{DateTime.Now.ToString()}\n{e.Message}");
            }
            finally
            {
            }

            //aTimer.Stop();
            //aTimer.Dispose();
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
        }
    }
}
