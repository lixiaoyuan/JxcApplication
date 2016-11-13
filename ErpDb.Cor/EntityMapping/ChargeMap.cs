using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ChargeMap : EntityTypeConfiguration<Charge>
    {
        public ChargeMap()
        {
            this.ToTable("Charge");
            this.HasKey(e => e.Id);
            this.Property(e => e.SumPrice)
              .HasPrecision(19, 4);

            this.Property(e => e.DiscountPrice)
                  .HasPrecision(19, 4);

            this.Property(e => e.RealPrice)
                   .HasPrecision(19, 4);

            this.Property(e => e.Remark)
                 .IsUnicode(false);
        }
    }
}
