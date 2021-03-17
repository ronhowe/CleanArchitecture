using Library.Sdk;
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

            Tip.Run();
        }
    }
}
