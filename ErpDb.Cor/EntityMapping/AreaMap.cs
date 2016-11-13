using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class AreaMap : EntityTypeConfiguration<Area>
    {
        public AreaMap()
        {
            this.ToTable("Area");
            this.HasKey(e => e.Id);
            this.Property(e => e.Name)
                .IsUnicode(false)
                .HasMaxLength(50);
        }
    }
}
