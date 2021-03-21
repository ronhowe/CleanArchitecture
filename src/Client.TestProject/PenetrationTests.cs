using FluentAssertions;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Solution.Client.TestProject
{
    using static Testing;

    public class PenetrationTests : TestBase
    {
        private const string uri = "https://api.ididevsecops.net";

        [Test]
        public async Task TestNetConnection()
        {
            HttpResponseMessage response = await GetHttpResponseMessageThirdPartyAddressAsAnonymous();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task TestAppConnection()
        {
            HttpResponseMessage response = await GetHttpResponseMessageHealthAddressAsAnonymous();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task TestAppHeaderForFrontDoor()
        {
            HttpResponseMessage response = await GetHttpResponseMessage(new Uri(uri), null);
            response.Headers.Where(e => e.Key == "fd-dev-idso-000").Should().HaveCount(1);
            Trace.WriteLine(response.Headers.Where(e => e.Key == "fd-dev-idso-000").FirstOrDefault().Value.FirstOrDefault());
        }

        [Test]
        public async Task TestAppHeaderForApiManagement()
        {
            HttpResponseMessage response = await GetHttpResponseMessage(new Uri(uri), null);
            response.Headers.Where(e => e.Key == "apim-dev-idso-000").Should().HaveCount(1);
            Trace.WriteLine(response.Headers.Where(e => e.Key == "apim-dev-idso-000").FirstOrDefault().Value.FirstOrDefault());
        }

        //[Test]
        //public async Task ServiceReturns401()
        //{
        //    HttpResponseMessage response = await GetHttpResponseMessageBaseAddressAsAnonymous();
        //    response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        //}

        //[Test]
        //public async Task ServiceReturns200()
        //{
        //    HttpResponseMessage response = await GetHttpResponseMessageBaseAddressAsAdmin();
        //    response.StatusCode.Should().Be(HttpStatusCode.OK);
        //}
    }
}
