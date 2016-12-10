using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor.EntityMapping
{
    public class FileCabinetsMap:EntityTypeConfiguration<FileCabinets>
    {
        public FileCabinetsMap()
        {
            this.ToTable("FileCabinets");
            this.HasKey(a => a.Id);
            this.Property(a => a.FilName).IsUnicode(false).HasMaxLength(200);
        }
    }
}
