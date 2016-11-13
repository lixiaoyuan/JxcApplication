using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ErpDb.Cor.Test
{
    [TestClass]
    public class TestMain
    {
        [TestMethod]
        public void Test1()
        {
            decimal? dec = 0;
            dec -= 2;
            Console.WriteLine(dec);
        }
         
    }
}
