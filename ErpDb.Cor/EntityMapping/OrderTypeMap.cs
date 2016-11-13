using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class OrderTypeMap : EntityTypeConfiguration<OrderType>
    {
        public OrderTypeMap()
        {
            this.ToTable("OrderType");
            this.HasKey(e => e.Id);

            this.Property(e => e.Code)
                .HasMaxLength(10);

            this.Property(e => e.Name)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(e => e.ReportUri)
                .IsUnicode(false)
                .HasMaxLength(500);

            this.Property(e => e.Remark)
                 .IsUnicode(false)
                 .HasMaxLength(200);
        }
    }
}
