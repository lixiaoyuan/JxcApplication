using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class AccountRecordMap : EntityTypeConfiguration<AccountRecord>
    {
        public AccountRecordMap()
        {
            this.ToTable("AccountRecord");
            this.HasKey(e => e.Id);
            this.Property(e => e.TransactionBefore).HasColumnType("money").HasPrecision(19,4);
            this.Property(e => e.TransactionBalance).HasColumnType("money").HasPrecision(19, 4);
            this.Property(e => e.TransactionAfter).HasColumnType("money").HasPrecision(19, 4);
            this.Property(e => e.CreateIp).HasMaxLength(20).IsUnicode(false);
        }
    }
}
