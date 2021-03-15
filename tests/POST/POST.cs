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
        public void Boot()
        {
            Debug.WriteLine(DateTime.Now);
        }
    }
}
