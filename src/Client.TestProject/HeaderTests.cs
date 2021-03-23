using FluentAssertions;
using NUnit.Framework;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Solution.Client.TestProject
{
    using static Testing;

    public class Header : TestBase
    {
        private static void AssertHeader(HttpResponseMessage response, string key)
        {
            response.Headers.Where(e => e.Key == key).Should().HaveCount(1);
            response.Headers.Where(e => e.Key == key).First().Value.Should().HaveCount(1);
            response.Headers.Where(e => e.Key == key).First().Value.First().Should().NotBeNullOrEmpty();
            Trace.WriteLine($"header={response.Headers.Where(e => e.Key == key).First().Value.First()}");
        }

        private static void AssertHeader(HttpResponseMessage response, string key, string value)
        {
            AssertHeader(response, key);
            response.Headers.Where(e => e.Key == key).First().Value.First().Should().NotBeNullOrEmpty();
            response.Headers.Where(e => e.Key == key).First().Value.First().Should().Be(value);
        }

        [Test]
        public async Task TestApplicationHeader()
        {
            HttpResponseMessage response = await GetHttpResponseMessageFromApplicationEndpoint();
            AssertHeader(response, Configuration["ApplicationHeaderKey"]);
        }

        [Test]
        public async Task TestAppServiceHeader()
        {
            HttpResponseMessage response = await GetHttpResponseMessageFromApplicationEndpoint();
            AssertHeader(response, Configuration["AppServiceHeaderKey"], Configuration["AppServiceHeaderValue"]);
        }

        [Test]
        public async Task TestApiManagementHeader()
        {
            HttpResponseMessage response = await GetHttpResponseMessageFromApiManagementEndpoint();
            AssertHeader(response, Configuration["ApiManagementHeaderKey"], Configuration["ApiManagementHeaderValue"]);
        }

        [Test]
        public async Task TestFrontDoorHeader()
        {
            HttpResponseMessage response = await GetHttpResponseMessageFromApplicationEndpoint();
            AssertHeader(response, Configuration["FrontDoorHeaderKey"], Configuration["FrontDoorHeaderValue"]);
        }
    }
}
