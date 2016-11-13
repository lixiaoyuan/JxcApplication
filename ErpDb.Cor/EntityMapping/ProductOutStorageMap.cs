using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ProductOutStorageMap:EntityTypeConfiguration<ProductOutStorage>
    {
        public ProductOutStorageMap()
        {
            this.ToTable("ProductOutStorage");
            this.HasKey(e => e.Id);

            this.Property(e => e.OrderType)
                .IsUnicode(false)
                .HasMaxLength(4)
                .IsRequired();

            this.Property(e => e.Code)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(e => e.GiveAddress)
                .IsUnicode(false)
                .HasMaxLength(300);

            this.Property(e => e.GiveArea)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(e => e.AcontackTel)
                .IsUnicode(false)
                .HasMaxLength(60);

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

            this.Property(e => e.AcontackName)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.HasMany(e => e.ProductOutStorageDetail)
                .WithOptional(e => e.ProductOutStorage)
                .HasForeignKey(e => e.PutOutId);
        }
    }
}
