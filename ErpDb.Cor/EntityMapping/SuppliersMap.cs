using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class SuppliersMap : EntityTypeConfiguration<Suppliers>
    {
        public SuppliersMap()
        {
            this.ToTable("Suppliers");
            this.HasKey(e => e.Id);


            this.Property(e => e.Code)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(e => e.Name)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(e => e.AllName)
                .IsUnicode(false)
                .HasMaxLength(100);

            this.Property(e => e.ZipCode)
                .IsUnicode(false)
                .HasMaxLength(10);

            this.Property(e => e.Fax)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(e => e.AContact)
                .IsUnicode(false)
                .HasMaxLength(10);

            this.Property(e => e.AContactPhone)
                .IsUnicode(false)
                .HasMaxLength(20);

            this.Property(e => e.AContactQq)
                .IsUnicode(false)
                .HasMaxLength(15);

            this.Property(e => e.AContactEmail)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(e => e.AContactAddress)
                .IsUnicode(false)
                .HasMaxLength(300);
        }
    }
}
