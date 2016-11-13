using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor.EntityMapping
{
    public class WorkApprovalOrderMap:EntityTypeConfiguration<WorkApprovalOrder>
    {
        public WorkApprovalOrderMap()
        {
            this.ToTable("WorkApprovalOrder");
            this.HasKey(a => a.Id);

            this.Property(a => a.UserName)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(a => a.OrderResultType)
                .IsUnicode(false)
                .HasMaxLength(30);

            this.Property(a => a.OrderStatusType)
              .IsUnicode(false)
              .HasMaxLength(30);

            this.Property(a => a.Comment)
                .IsUnicode(false)
                .HasMaxLength(6000);

            this.Property(a => a.FormData)
                .HasMaxLength(8000);

            this.HasRequired(b => b.WorkApproval)
                .WithMany()
                .HasForeignKey(b => b.WorkApprovalId);
        }
    }
}
