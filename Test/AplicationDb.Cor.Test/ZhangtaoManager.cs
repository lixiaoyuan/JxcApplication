using System;
using ApplicationDb.Cor.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AplicationDb.Cor.Test
{
    [TestClass]
    public class ZhangtaoManager
    {
        [TestMethod]
        public void GetZhangtaostring()
        {
            ApplicationDb.Cor.Business.ZhangtaoManager z= ApplicationDb.Cor.Business.ZhangtaoManager.Create();
            var connectstring=z.GetConnectString(Guid.Parse("E5DB29F4-C34D-4C4E-9FA7-E703FC317EE7"));
            Console.WriteLine(connectstring);
        }

    }
}
