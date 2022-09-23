using Microsoft.VisualStudio.TestTools.UnitTesting;
using Biz.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static Biz.Common.SQLCodeHelper;

namespace Biz.Common.Tests
{
    [TestClass()]
    public class SQLCodeHelperTests
    {
        [TestMethod()]
        public void AnalyseTest()
        {
            if (!File.Exists("codes.txt"))
            {
                File.Create("codes.txt").Close();
            }
            var codes = File.ReadAllText("codes.txt");
            var results = SQLCodeHelper.Analyse(codes.ToArray(), 0);
            foreach (var r in results)
            {
                if (r.Type == AnalyseType.Token)
                {
                    System.Diagnostics.Trace.WriteLine(r.Type + ":" + codes.Substring(r.StartIndex, r.EndIndex - r.StartIndex + 1));
                }
            }
        }
    }
}