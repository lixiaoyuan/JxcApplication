using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ProductTypeMap : EntityTypeConfiguration<ProductType>
    {
        public ProductTypeMap()
        {
            this.ToTable("ProductType");
            this.HasKey(e => e.Id);


            this.Property(e => e.Code)
                .IsUnicode(false)
                .HasMaxLength(4);

            this.Property(e => e.Name)
                .IsUnicode(false)
                .HasMaxLength(50);
        }
    }
}
