using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ProductOutInStorageDetailMap:EntityTypeConfiguration<ProductOutInStorageDetail>
    {
        public ProductOutInStorageDetailMap()
        {
            this.ToTable("ProductOutInStorageDetail");
            this.HasKey(e => e.Id);

            this.Property(e => e.OriginalStock)
                .HasPrecision(10, 2);

            this.Property(e => e.SubtractStock)
                .HasPrecision(10, 2);

            this.Property(e => e.LasetStock)
                .HasPrecision(10, 2);

            this.Property(e => e.CreateIp)
                .IsUnicode(false)
                .HasMaxLength(20);
        }
    }
}
