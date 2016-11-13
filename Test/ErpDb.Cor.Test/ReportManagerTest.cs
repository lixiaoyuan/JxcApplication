using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ErpDb.Cor.Test
{
    [TestClass]
    public class ReportManagerTest
    {
        private ReportManager _reportManager;


        [TestInitialize]
        public void Init()
        {
            _reportManager=ReportManager.Create();
        }

        [TestMethod]
        public void QueStoreProductStocks()
        {
            var result = _reportManager.QueStoreProductStocks(Guid.Parse("D66C26BD-3BEB-4242-8346-0A4980BA6FC7"));
            Assert.IsNotNull(result);
        }
    }
}
