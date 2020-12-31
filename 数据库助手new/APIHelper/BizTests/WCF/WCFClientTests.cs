using Microsoft.VisualStudio.TestTools.UnitTesting;
using Biz.WCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biz.WCF.Tests
{
    [TestClass()]
    public class WCFClientTests
    {
        [TestMethod()]
        public void GetInterfaceInfosTest()
        {
            var client = WCFClient.CreateClient("http://10.252.254.104:13113/Service.svc");

            var ops = client.GetInterfaceInfos();
        }
    }
}