using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessDb.Cor.Test
{
    [TestClass]
    public class ApplicationDbContextFunctionTest
    {
        [TestMethod]
        public void Report_PaymentedV2()
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                var d=db.Report_PaymentedV2(DateTime.Now.AddMonths(-10), DateTime.Now, "NotPaymentModel");
                foreach (object o in d)
                {
                    Console.WriteLine(o);
                }
            }
        }
    }
}
