using Microsoft.VisualStudio.TestTools.UnitTesting;
using Biz.Common.SqlAnalyse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Biz.Common.SqlAnalyse.Tests
{
    [TestClass()]
    public class SqlProcessorTests
    {
        [TestMethod()]
        public void HandleTest()
        {
            if (!File.Exists("codes.txt"))
            {
                File.Create("codes.txt").Close();
            }
            var codes = File.ReadAllText("codes.txt");
            var reader = new SqlReader(codes);

            var results = new SqlProcessor(codes).Handle();
        }
    }
}