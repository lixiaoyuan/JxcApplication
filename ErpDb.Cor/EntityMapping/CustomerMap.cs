using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class CustomerMap:EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            this.ToTable("Customer");
            this.HasKey(e => e.Id);

            this.Property(e => e.Code)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(e => e.Name)
                .IsUnicode(false)
                .HasMaxLength(200);

            this.Property(e => e.Tel)
                .IsUnicode(false)
                .HasMaxLength(50);

            this.Property(e => e.Fax)
                .IsUnicode(false)
                .HasMaxLength(30);

            this.Property(e => e.Area)
                .IsUnicode(false)
                .HasMaxLength(150);

            this.Property(e => e.Address)
                .IsUnicode(false)
                .HasMaxLength(500);

            this.Property(e => e.Credibility)
                .HasPrecision(19, 4)
                .HasColumnType("money");

            this.Property(e => e.AcontactName)
                .IsUnicode(false)
                .HasMaxLength(30);

            this.Property(e => e.AcontactTel)
                .IsUnicode(false)
                .HasMaxLength(100);

            this.Property(e => e.Balance)
                .HasPrecision(19, 4)
                .HasColumnType("money");
        }
    }
}
