using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationDb.Cor.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AplicationDb.Cor.Test
{
    [TestClass]
    public class FileCabinetsManagerTest
    {
        [TestMethod]
        public void InserFile()
        {
            byte[] tt = File.ReadAllBytes(@"D:\外接系统\厦门比格维尔\文档\月计划模板.doc");
            var id = FileCabinetsManager.InserFile(tt, "月计划模板.doc");
            Console.WriteLine(id);
        }
    }
}
