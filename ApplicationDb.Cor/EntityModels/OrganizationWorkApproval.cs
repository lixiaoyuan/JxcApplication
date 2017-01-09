using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.EntityModels
{
    /// <summary>
    /// 部门可见审批
    /// </summary>
    public class OrganizationWorkApproval
    {
        public Guid Id { get; set; }
        public Guid OrganizationId { get; set; }
        public Guid WorkApprovalId { get; set; }
    }
}
