using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessDb.Cor.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ErpDb.Cor.Test
{
    [TestClass]
   public class ProductManagerTest
    {
        private ProductManager _productManager;

        [TestInitialize]
        public void Init()
        {
            _productManager=ProductManager.Create();
        }

        [TestMethod]
        public void QueryStoteId()
        {
            var result = ProductManager.QueryByStore(Guid.Parse("D66C26BD-3BEB-4242-8346-0A4980BA6FC7"));
            Assert.IsTrue(result.Any());
        }
    }
}
