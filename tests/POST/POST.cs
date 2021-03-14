using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace POST
{
    [TestClass]
    public class POST
    {
        [TestMethod]
        public void Post()
        {
            Debug.WriteLine("POST");
            Assert.IsTrue(true);
            PostBoot();
        }

        private static void PostBoot()
        {
            Debug.WriteLine("your code here");
        }
    }
}
