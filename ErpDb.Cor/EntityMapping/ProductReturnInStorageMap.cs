using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ProductReturnInStorageMap : EntityTypeConfiguration<ProductReturnInStorage>
    {
        public ProductReturnInStorageMap()
        {
            this.ToTable("ProductReturnInStorage");
            this.HasKey(e => e.Id);

            this.Property(e => e.OrderType)
                .IsUnicode(false)
                .HasMaxLength(4);

            this.Property(e => e.Code)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(e => e.PaymentType)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(e => e.SumPrice)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.Paid)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.Remark)
                .IsUnicode(false)
                .HasMaxLength(200);

            this.Property(e => e.CreateIp)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(e => e.ModiftyIp)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.HasMany(e => e.ProductInStorageDetail)
                .WithOptional(e => e.ProductReturnInStorage)
                .HasForeignKey(e => e.ReturnInId);
        }
    }
}
