using NUnit.Framework;
using System.Threading.Tasks;

namespace Solution.Client.TestProject
{
    using static Testing;

    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await ResetState();
        }
    }
}
