using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ProductInStorageMap : EntityTypeConfiguration<ProductInStorage>
    {
        public ProductInStorageMap()
        {
            this.ToTable("ProductInStorage");
            this.HasKey(e => e.Id);

            this.Property(e => e.OrderType)
                .IsUnicode(false)
                .HasMaxLength(4)
                .IsRequired();

            this.Property(e => e.Code)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(e => e.UnitsPurchased)
                .IsUnicode(false)
                .HasMaxLength(200);

            this.Property(e => e.Factory)
                .IsUnicode(false)
                .HasMaxLength(300);

            this.Property(e => e.SumPrice)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.Remark)
                .IsUnicode(false)
                .HasMaxLength(300);

            this.Property(e => e.CreteIp)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(e => e.ModiftyIp)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.HasMany(e => e.ProductInStorageDetail)
                .WithOptional(e => e.ProductInStorage)
                .HasForeignKey(e => e.PutInId);
        }
    }
}
