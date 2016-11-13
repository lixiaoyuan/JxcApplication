using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.EntityModels
{
    public class WorkApprovalCopyUser
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? WorkApprovalId { get; set; }
        public int Sort { get; set; }

        public virtual ICollection<SystemUser> SystemUsers { get; set; }
        public virtual ICollection<WorkApproval> WorkApprovals { get; set; }
    }
}
