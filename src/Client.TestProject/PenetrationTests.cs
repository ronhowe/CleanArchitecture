using FluentAssertions;
using NUnit.Framework;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Solution.Client.TestProject
{
    using static Testing;

    public class PenetrationTests : TestBase
    {
        [Test]
        public async Task HealthCheckReturns200()
        {
            HttpResponseMessage response = await GetHttpResponseMessageHealthAddressAsAnonymous();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task ServiceReturns401()
        {
            HttpResponseMessage response = await GetHttpResponseMessageBaseAddressAsAnonymous();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        }

        [Test]
        public async Task ServiceReturns200()
        {
            HttpResponseMessage response = await GetHttpResponseMessageBaseAddressAsAdmin();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
