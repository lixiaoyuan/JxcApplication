using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ProductMap:EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            this.ToTable("Product");
            this.HasKey(e => e.Id);

            this.Property(e => e.ProductType)
                .IsUnicode(false)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(e => e.Code)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(e => e.Name)
                .IsUnicode(false)
                .HasMaxLength(200);

            this.Property(e => e.PyCode)
                .IsUnicode(false)
                .HasMaxLength(200);

            this.Property(e => e.Specification)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(e => e.Unit)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(e => e.UnitPrice)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.StockRemind)
                .HasPrecision(10, 2);

            this.Property(e => e.Manufacturer)
                .IsUnicode(false)
                .HasMaxLength(200);
        }
    }
}
