using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor.EntityMapping
{
    public class WorkApprovalOrderCopyUserMap:EntityTypeConfiguration<WorkApprovalOrderCopyUser>
    {
        public WorkApprovalOrderCopyUserMap()
        {
            this.ToTable("WorkApprovalOrderCopyUser");
            this.HasKey(a => a.Id);

            this.Property(a => a.Comment)
                .IsUnicode(false)
                .HasMaxLength(300);

            this.HasRequired(a => a.WorkApprovalOrder)
                .WithMany()
                .HasForeignKey(a => a.WorkApprovalOrderId);

            this.HasRequired(a => a.CopyUser)
                .WithMany()
                .HasForeignKey(a => a.UserId);
        }
    }
}
