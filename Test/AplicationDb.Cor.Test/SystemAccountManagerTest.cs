using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationDb.Cor.Business;
using BusinessDb.Cor.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AplicationDb.Cor.Test
{
    [TestClass]
    public class SystemAccountManagerTest
    {
        [TestInitialize]
        public void TestInit()
        {
        }
        [TestMethod]
        public void QueryLookUp()
        {
            SystemAccountManager.QueryLookUp();
        }
    }
}
