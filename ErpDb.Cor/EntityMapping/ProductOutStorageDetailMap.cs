using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ProductOutStorageDetailMap : EntityTypeConfiguration<ProductOutStorageDetail>
    {
        public ProductOutStorageDetailMap()
        {
            this.ToTable("ProductOutStorageDetail");
            this.HasKey(e => e.Id);


           this.Property(e => e.OrderType)
                .IsUnicode(false)
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

            this.Property(e => e.OutStock)
                .HasPrecision(10, 2);

            this.Property(e => e.UnitPrice)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.SumPrice)
                .HasPrecision(19, 4)
                .HasColumnType("money");


            this.HasMany(e => e.ProductOutInStorageDetail)
                .WithOptional(e => e.ProductOutStorageDetail)
                .HasForeignKey(e => e.PutOutStorageId);
        }
    }
}
