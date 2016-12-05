using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor.EntityMapping
{
    public class MailOrderMap:EntityTypeConfiguration<MailOrder>
    {
        public MailOrderMap()
        {
            this.ToTable("MailOrder");
            this.HasKey(a => a.Id);
            this.Property(a => a.FormAddress).IsUnicode(false).HasMaxLength(30);
            this.Property(a => a.FormUserName).IsUnicode(false).HasMaxLength(30);
            this.Property(a => a.ToAddress).IsUnicode(false).HasMaxLength(30);
            this.Property(a => a.ToUserName).IsUnicode(false).HasMaxLength(30);
            this.Property(a => a.Subject).HasMaxLength(100).IsUnicode(false);

        }
    }
}
