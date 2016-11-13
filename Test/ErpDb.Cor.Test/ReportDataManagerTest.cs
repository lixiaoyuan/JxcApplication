using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessDb.Cor.Business;
using BusinessDb.Cor.Helper;
using BusinessDb.Cor.Models.ReportDataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ErpDb.Cor.Test
{
    [TestClass]
    public class ReportDataManagerTest
    {
        [TestInitialize]
        public void InitConnectString()
        {
            try
            {
                ConnectStringHelper.ConnectString = "metadata=res://*/Application_First.csdl|res://*/Application_First.ssdl|res://*/Application_First.msl;provider=System.Data.SqlClient;provider connection string=\"data source =.; initial catalog = Application_First; persist security info = True; user id = sa; password = 123456; MultipleActiveResultSets = True; App = EntityFramework\"";
            }
            catch (Exception)
            {
            }
        }

        [TestMethod]
        public void GetOutProductData()
        {
           var result= ReportDataManager.Instances.GetOutProductData(Guid.Parse("462D7B25-0CF7-4D93-9011-821549FD65C2"));
            Assert.IsNotNull(result);
            foreach (OutProductData data in result)
            {
                Console.WriteLine(data.Title);
                Assert.IsNotNull(data.Details);
                foreach (OutProductData.Detail detail in data.Details)
                {
                    Console.WriteLine(detail);
                }
            }
           
        }
        [TestMethod]
        public void GetProductInStroeData()
        {
            var result = ReportDataManager.Instances.GetProductInStroeData(Guid.Parse("470FA3CF-0A6E-4EBD-94C8-1BAB33DDEA77")).ToList();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetChargeData()
        {
            var result = ReportDataManager.Instances.GetChargeData(Guid.Parse("3A4ABA4E-224E-43F7-8DAA-22C0F7E1A642")).ToList();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result[0].Title);
            Assert.IsNotNull(result[0].Details);
            Assert.IsTrue(result[0].Details.ToList().Any());
        }

        [TestMethod]
        public void GetProductReturnData()
        {
            var result = ReportDataManager.Instances.GetProductReturnData(Guid.Parse("1D49782B-8D63-430B-845A-560031B3EE89")).ToList();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result[0].Title);
            Assert.IsNotNull(result[0].Details);
            Assert.IsTrue(result[0].Details.ToList().Any());
        }
    }
}
