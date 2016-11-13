using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class GenerateOrderMap:EntityTypeConfiguration<GenerateOrder>
    {
        public GenerateOrderMap()
        {
            this.ToTable("GenerateOrder");
            this.HasKey(e => e.Id);

            this.Property(e => e.Type)
               .IsUnicode(false);

            this.Property(e=>e.GenerateTime)
                .HasColumnType("date");
        }
    }
}
