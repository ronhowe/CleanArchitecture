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

    public class EndpointTests : TestBase
    {
        [Test]
        public async Task TestInternetEndpoint()
        {
            HttpResponseMessage response = await GetHttpResponseMessageFromInternetEndpoint();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task TestApplicationEndpoint()
        {
            HttpResponseMessage response = await GetHttpResponseMessageFromApplicationEndpoint();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task TestFrontDoorEndpoint()
        {
            HttpResponseMessage response = await GetHttpResponseMessageFromFrontDoorEndpoint();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task TestApiManagementEndpoint()
        {
            HttpResponseMessage response = await GetHttpResponseMessageFromApiManagementEndpoint();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
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
