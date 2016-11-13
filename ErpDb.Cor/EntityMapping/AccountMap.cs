using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class AccountMap : EntityTypeConfiguration<Account>
    {
        public AccountMap()
        {
            this.ToTable("Account");
            this.HasKey(e => e.Id);
            this.Property(e => e.Code).IsUnicode(false).HasMaxLength(20);
            this.Property(e => e.Name).IsUnicode(false).HasMaxLength(50);
            this.Property(e => e.CreateIp).IsUnicode(false).HasMaxLength(20);
            this.Property(e => e.Balance).HasPrecision(19,4).HasColumnType("money");
        }
    }
}
