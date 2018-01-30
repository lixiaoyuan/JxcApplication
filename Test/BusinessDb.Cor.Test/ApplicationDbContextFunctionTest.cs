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
                var d=db.Report_PaymentedV2(DateTime.Now.AddMonths(-10), DateTime.Now, "NotPaymentModel",Guid.Empty);
                foreach (object o in d)
                {
                    Console.WriteLine(o);
                }
            }
        }

        [TestMethod]
        public void CheckLockAmount()
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                string i = "";
                var allow=db.CheckLockAmount(Guid.Parse("D66C26BD-3BEB-4242-8346-0A4980BA6FC7"),
                    Guid.Parse("9820B142-F507-4A6A-815E-18F361B523C1"), 20, out i);
                Console.WriteLine("{0}-{1}",allow,i);

            }
        }
    }
}
