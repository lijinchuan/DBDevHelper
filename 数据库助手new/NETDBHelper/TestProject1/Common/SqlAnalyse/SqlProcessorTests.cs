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
            if (!File.Exists("codes.sql"))
            {
                File.Create("codes.sql").Close();
            }
            var codes = File.ReadAllText("codes.sql");
            var reader = new SqlReader(codes);

            var results = new SqlCodeDom(codes).Analyse();

            foreach(var r in results)
            {
                r.Print(codes);
                
            }
        }
    }
}