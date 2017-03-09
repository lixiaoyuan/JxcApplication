using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities;

namespace AplicationDb.Cor.Test
{
    [TestClass]
    public class TestEnter
    {
        public TestEnter()
        {
            //ApplicationDb.Cor.Helper.ConnectStringHelper.ConnectString = "metadata=res://*/AppModel.csdl|res://*/AppModel.ssdl|res://*/AppModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=.;initial catalog=ApplicationDb;persist security info=True;user id=sa;password=123456;MultipleActiveResultSets=True;App=EntityFramework\"";
        }

        [TestMethod]
        public void TestEntityUpdate()
        {
            //ApplicationDb.Cor.TestEnter t=new  ApplicationDb.Cor.TestEnter();
            //t.TestEntityUpdate();
        }

        [TestMethod]
        public void Jiami()
        {//加密
            DESCryp  desCryp=new DESCryp();
            //Console.WriteLine(desCryp.Encrypt("metadata=res://*/AppModel.csdl|res://*/AppModel.ssdl|res://*/AppModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=.;initial catalog=ApplicationDb;persist security info=True;user id=sa;password=123456;MultipleActiveResultSets=True;App=EntityFramework\"","1qaz!QAZ"));
#if DEBUG
            Console.WriteLine(desCryp.Encrypt("data source=192.1.40.79;initial catalog=ApplicationDb;user id=sa;password=123456;MultipleActiveResultSets=True;App=EntityFramework", "1qaz!QAZ"));
#else
            Console.WriteLine(desCryp.Encrypt("data source=192.168.1.201;initial catalog=ApplicationDb;user id=sa;password=1qaz!QAZ;MultipleActiveResultSets=True;App=EntityFramework", "1qaz!QAZ"));
#endif
        }

        [TestMethod]
        public void Jiemi()
        {
            DESCryp desCryp=new DESCryp();
            Console.WriteLine(desCryp.Decrypt("d0Pum688+uriiW5oa/oW8jSiCd1FaLpggeWt3SYp2B+q/NNiq5t3XxSXGDjsDYSAwif2/RabkYi72UjdrQBpEkRBOSXe4b991ZRSCLEEzV0UrNLmSi2Wk6LAXobHLS9RA/nTNRsTYWfkMG514fUUAlkgPk6LOvAgfcY/auc0Dqk=", "1qaz!QAZ"));
        }

        [TestMethod]
        public void TestMenuManager()
        {
            //ApplicationDb.Cor.Business.MenuManager menuManager = ApplicationDb.Cor.Business.MenuManager.Create();
            //var list=menuManager.GetRoleMenuCheck(Guid.Parse("355FB643-57D6-4D7D-B747-0CBE835280BD"), "erp").ToList();
            //Assert.IsTrue(list.Any());
        }
    }
}
