using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ChargeDetailsMap : EntityTypeConfiguration<ChargeDetails>
    {
        public ChargeDetailsMap()
        {
            this.ToTable("ChargeDetails");
            this.HasKey(e => e.Id);
            this.Property(e => e.OrderType)
                .IsUnicode(false)
                .HasMaxLength(4);
            this.Property(e => e.RefCode)
                .HasMaxLength(20);
            this.Property(e => e.SumPrice)
                  .HasPrecision(19, 4)
                  .HasColumnType("money");

            this.Property(e => e.Paid)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.LastPrice)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.ThisPrice)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.Remark)
                .IsUnicode(false)
                .HasMaxLength(200);
        }
    }
}
