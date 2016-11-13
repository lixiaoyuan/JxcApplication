using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationDb.Cor.Business;
using ApplicationDb.Cor.EntityModels;
using ApplicationDb.Cor.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AplicationDb.Cor.Test
{
    [TestClass]
    public class RibbonNodeManagerTest
    {
        [TestMethod]
        public void RiibonNodeConvert()
        {
            AuthRibbonNode node=new AuthRibbonNode();
            node.Caption = "待转换数据";
            node.Color = "#F6933333";
            RibbonNodeModel mode = node;
            Console.WriteLine(mode.Caption);
        }

        [TestMethod]
        public  void GetAuthRibbon()
        {
            RibbonNodeManager ribbon = RibbonNodeManager.Create();
            var data = ribbon.GetAuthRibbonAsync(Guid.Parse("6A0307D3-7E12-4048-B5AD-BE02BFB440C5"),
                Guid.Parse("FA38CF52-CAFC-43DF-BA63-92958B8E3C5E"), "erp").Result;
            Assert.IsNotNull(data);
        }
         
    }
}
