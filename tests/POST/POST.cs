using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace POST
{
    [TestClass]
    public class POST
    {
        [TestMethod]
        public void Post()
        {
            Debug.WriteLine("POST");
            Debug.WriteLine(DateTime.Now);
            Assert.IsTrue(true);
            PostBoot();
        }

        private static void PostBoot()
        {
            int loop = 100;
            for (int i = 0; i < loop; i++)
            {
                if ((i % 5) == 0) {
                    Debug.WriteLine("-");
                }
                else
                {
                    Debug.WriteLine("|");
                }
            }
        }
    }
}
