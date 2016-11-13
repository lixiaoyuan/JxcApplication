using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utilities;

namespace ErpDb.Cor.Test
{
    [TestClass]
    public class DescrypTest
    {
        [TestMethod]
        public void Jiami()
        {
            //192.168.1.201
            //string l= "metadata = res://*/AppModel.csdl|res://*/AppModel.ssdl|res://*/AppModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=192.168.1.201;initial catalog=ApplicationDb;persist security info=True;user id=sa;password=1qaz!QAZ;MultipleActiveResultSets=True;App=EntityFramework\"";
            string l = "metadata = res://*/AppModel.csdl|res://*/AppModel.ssdl|res://*/AppModel.msl;provider=System.Data.SqlClient;provider connection string=\"data source=.;initial catalog=ApplicationDb;persist security info=True;user id=sa;password=123456;MultipleActiveResultSets=True;App=EntityFramework\"";

            DESCryp desCryp = new DESCryp();
            Console.WriteLine(desCryp.Encrypt(l, "1qaz!QAZ"));
            
        }
    }
}
