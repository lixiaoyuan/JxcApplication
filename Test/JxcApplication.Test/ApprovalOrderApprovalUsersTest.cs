using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JxcApplication.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JxcApplication.Test
{
    [TestClass]
    public class ApprovalOrderApprovalUsersTest
    {
        [TestMethod]
        public void CreateApprovalUsers()
        {
            Guid nextUserId;
            var xml=  ApprovalOrderCor.CreateApprovalUsers(Guid.Parse("e994d0a8-35f6-49ab-8579-65c88b92a642"),out nextUserId);
            Console.WriteLine(nextUserId);
            Console.WriteLine(xml);
        }
    }
}
