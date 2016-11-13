using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class AcontactMap : EntityTypeConfiguration<Acontact>
    {
        public AcontactMap()
        {
            this.ToTable("Acontact");
            this.HasKey(e => e.Id);
            this.Property(e => e.Code).IsUnicode(false).HasMaxLength(20);
            this.Property(e => e.Name).IsUnicode(false).HasMaxLength(30);
            this.Property(e => e.Tel).IsUnicode(false).HasMaxLength(50);
            this.Property(e => e.Phone).IsUnicode(false).HasMaxLength(50);
            this.Property(e => e.Qq).IsUnicode(false).HasMaxLength(50);
            this.Property(e => e.Email).IsUnicode(false).HasMaxLength(50);
            this.Property(e => e.Area).IsFixedLength().HasMaxLength(10);
            this.Property(e => e.Address).IsUnicode(false).HasMaxLength(300);
        }
    }
}
