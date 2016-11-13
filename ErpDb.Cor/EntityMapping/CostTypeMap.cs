using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.EntityMapping
{
    public class CostTypeMap:EntityTypeConfiguration<EntityModels.CostType>
    {
        public CostTypeMap()
        {
            this.ToTable("CostType");
            this.HasKey(a => a.Id);
            this.Property(a => a.Name).IsUnicode(false).HasMaxLength(100);
        }

    }
}
