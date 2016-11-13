using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessDb.Cor;
using BusinessDb.Cor.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ErpDb.Cor.Test
{
    [TestClass]
    public class ApplicationDbContextFunctionTest
    {
        private ApplicationDbContext db;
        [TestInitialize]
        public void Initialize()
        {
            ConnectStringHelper.ConnectString =
                "data source=.;initial catalog=Application_First;user id=sa;password=123456;MultipleActiveResultSets=True;App=EntityFramework";
            db=new ApplicationDbContext();
        }
        [TestCleanup]
        public void Cleanup()
        {
            db.Dispose();
        }

        [TestMethod]
        public void GetOrderCode()
        {
            var result = db.GetOrderCode("XK").FirstOrDefault();
            Assert.IsNotNull(result);
            Console.WriteLine(result);
        }

        [TestMethod]
        public void GetProductInCanUpdate()
        {
            SqlParameter returnParameter = new SqlParameter("ReurnCanUpdate", SqlDbType.VarChar, 200);
            returnParameter.Direction = ParameterDirection.Output;
            var result = db.GetProductInCanUpdate(Guid.NewGuid(), returnParameter);
            Assert.IsNotNull(result);
            Console.WriteLine($"result:{result}");
            Assert.IsNotNull(returnParameter.Value);
            Console.WriteLine($"returnParameter:{returnParameter.Value}");
        }

    }
}
