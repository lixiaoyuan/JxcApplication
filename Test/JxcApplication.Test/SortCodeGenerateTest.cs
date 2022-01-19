using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JxcApplication.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JxcApplication.Test
{
    [TestClass]
    public class SortCodeGenerateTest
    {
        [TestMethod]
        public void Gen()
        {
            SortCodeGenerate g=new SortCodeGenerate();
            var p = SortCodeGenerate.GetCode("CP","CP999");
            Console.WriteLine(p);
        }
    }
}
