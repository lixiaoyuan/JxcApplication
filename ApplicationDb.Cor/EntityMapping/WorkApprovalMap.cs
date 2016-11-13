using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor.EntityMapping
{
    public class WorkApprovalMap : EntityTypeConfiguration<WorkApproval>
    {
        public WorkApprovalMap()
        {
            this.ToTable("WorkApproval");
            this.HasKey(a => a.Id);

            Property(a => a.Name)
                .IsUnicode(false)
                .HasMaxLength(20);

            Property(a => a.Remark)
                .IsUnicode(false)
                .HasMaxLength(100);

            Property(a => a.FormDataTemplate)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(a => a.DataType)
                .IsUnicode(false)
                .HasMaxLength(300);

            //this.HasMany(a => a.ApprovalUser)
            //    .WithMany(a => a.MainApprovals)
            //    .Map(m =>
            //    {
            //        m.MapLeftKey("ApprovalId");
            //        m.MapRightKey("UserId");
            //        m.ToTable("WorkApprovalUser");
            //    });

            //this.HasMany(a => a.CopyUser)
            //   .WithMany(a => a.CopyApprovals)
            //   .Map(m =>
            //   {
            //       m.MapLeftKey("ApprovalId");
            //       m.MapRightKey("UserId");
            //       m.ToTable("WorkApprovalCopyUser");
            //   });
        }
    }
}
