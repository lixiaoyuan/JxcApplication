using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor.EntityMapping
{
    public class WorkApprovalCopyUserMap : EntityTypeConfiguration<WorkApprovalCopyUser>
    {
        public WorkApprovalCopyUserMap()
        {
            this.ToTable("WorkApprovalCopyUser");
            this.HasKey(a => a.Id);

            this.HasRequired(b => b.SystemUsers)
              .WithMany()
              .HasForeignKey(b => b.UserId);

            this.HasRequired(b => b.WorkApprovals)
              .WithMany()
              .HasForeignKey(b => b.WorkApprovalId);
        }
    }
}
