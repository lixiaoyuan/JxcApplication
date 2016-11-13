using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ExpensesDetailMap: EntityTypeConfiguration<ExpensesDetail>
    {
        public ExpensesDetailMap()
        {
            this.ToTable("ExpensesDetail");
            this.HasKey(a => a.Id);
            this.Property(a => a.Mark).IsUnicode(false).HasMaxLength(300);
        }
    }
}
