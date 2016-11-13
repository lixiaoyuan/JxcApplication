using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class TransactionTypeMap: EntityTypeConfiguration<BusinessDb.Cor.EntityModels.TransactionType>
    {
        public TransactionTypeMap()
        {
            this.ToTable("TransactionType");
            this.HasKey(a => a.Id);
            this.Property(a => a.Code);
            this.Property(a => a.Name).IsUnicode(false).HasMaxLength(50);
        }
    }
}
