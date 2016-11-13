using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class WageMap:EntityTypeConfiguration<Wage>
    {
        public WageMap()
        {
            this.ToTable("Wage");
            this.HasKey(a => a.Id);

            this.Property(e => e.WageDate)
                .HasColumnType("date");

            this.Property(e => e.SumPrice)
                .HasPrecision(10, 2)
                .HasColumnType("money");

            this.Property(e => e.Remark)
                .IsUnicode(false)
                .HasMaxLength(100);

            this.Property(e => e.CreateIp)
               .IsUnicode(false)
               .HasMaxLength(20);

            this.Property(e => e.ModiftyIp)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.HasMany(a => a.WageDetail)
                .WithRequired(a => a.Wage)
                .HasForeignKey(a => a.WageId)
                .WillCascadeOnDelete(false);
        }
    }
}
