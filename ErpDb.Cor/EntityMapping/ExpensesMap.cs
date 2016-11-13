using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ExpensesMap: EntityTypeConfiguration<Expenses>
    {
        public ExpensesMap()
        {
            this.ToTable("Expenses");
            this.HasKey(a => a.Id);

            this.Property(a => a.Bill).IsUnicode(false).HasMaxLength(40);
            this.Property(a => a.CreteIp).IsUnicode(false).HasMaxLength(20);
            this.Property(a => a.ModiftyIp).IsUnicode(false).HasMaxLength(20);
            this.Property(a => a.SumMoney).HasColumnType("money");
            this.Property(a => a.Code)
                .IsUnicode(false)
                .HasMaxLength(20);
            this.Property(a => a.OrderType)
                .IsUnicode(false)
                .HasMaxLength(10);

            this.HasMany(a => a.ExpensesDetail)
                .WithRequired(a => a.Expenses)
                .HasForeignKey(a => a.ExpensesId)
                .WillCascadeOnDelete(false);
        }
    }
}
