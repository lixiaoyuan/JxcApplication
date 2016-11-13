using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class StoreMap : EntityTypeConfiguration<Store>
    {
        public StoreMap()
        {
            this.ToTable("Store");
            this.HasKey(e => e.Id);

            this.Property(e => e.Code)
              .IsUnicode(false)
              .HasMaxLength(2);

            this.Property(e => e.Name)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(e => e.Address)
                .IsUnicode(false)
                .HasMaxLength(200);

            this.Property(e => e.ProductType)
               .IsUnicode(false)
               .HasMaxLength(4);

            this.Property(e => e.ReMark)
                .IsUnicode(false)
                .HasMaxLength(300);

            this.HasMany(e => e.ProductInStorage)
                .WithOptional(e => e.Store)
                .HasForeignKey(e => e.StorageId);

        }
    }
}
