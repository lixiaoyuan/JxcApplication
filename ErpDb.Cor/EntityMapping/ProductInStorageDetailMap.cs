using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ProductInStorageDetailMap:EntityTypeConfiguration<ProductInStorageDetail>
    {
        public ProductInStorageDetailMap()
        {
            this.ToTable("ProductInStorageDetail");
            this.HasKey(e => e.Id);


            this.Property(e => e.OrderType)
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(e => e.ProductCode)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(e => e.ProductSpecification)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(e => e.ProductUnit)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(e => e.OriginalStock)
                .HasPrecision(10, 2);

            this.Property(e => e.LastStock)
                .HasPrecision(10, 2);

            this.Property(e => e.UnitPrice)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.SumPrice)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.Remark)
                .IsUnicode(false)
                .HasMaxLength(200);

            this.HasMany(e => e.ProductOutInStorageDetail)
                .WithOptional(e => e.ProductInStorageDetail)
                .HasForeignKey(e => e.PutInStorageId);
        }
    }
}
