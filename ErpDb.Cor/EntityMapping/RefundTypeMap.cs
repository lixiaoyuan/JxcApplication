using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class RefundTypeMap : EntityTypeConfiguration<RefundType>
    {
        public RefundTypeMap()
        {
            this.ToTable("RefundType");
            this.HasKey(a => a.Id);
            this.Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(a => a.Code).HasMaxLength(20).IsUnicode(false);
            this.Property(a => a.Name).HasMaxLength(20).IsUnicode(false);
        }
    }
}
