using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor.EntityMapping
{
    public class OrganizationMap : EntityTypeConfiguration<Organization>
    {
        public OrganizationMap()
        {
            ToTable("Organization");
            this.HasKey(a => a.Id);

            this.Property(a => a.Code)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(a => a.Name)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.HasOptional(a => a.Parent)
                .WithMany(a => a.Children)
                .HasForeignKey(d => d.ParentId);
        }
    }
}
