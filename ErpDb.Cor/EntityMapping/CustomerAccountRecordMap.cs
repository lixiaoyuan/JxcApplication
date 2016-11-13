using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class CustomerAccountRecordMap:EntityTypeConfiguration<CustomerAccountRecord>
    {
        public CustomerAccountRecordMap()
        {
            this.ToTable("CustomerAccountRecord");
            this.HasKey(e => e.Id);

           this.Property(e => e.TransactionBefore)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.TransactionBalance)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.TransactionAfter)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.CreateIp)
                .IsUnicode(false)
                .HasMaxLength(20);
        }
    }
}
