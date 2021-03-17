using Library.Sdk;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

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

            //Assert.IsTrue(Tip.Run());
        }
    }
}
