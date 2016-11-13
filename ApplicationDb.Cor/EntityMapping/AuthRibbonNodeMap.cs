using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor.EntityMapping
{
    public class AuthRibbonNodeMap : EntityTypeConfiguration<AuthRibbonNode>
    {
        public AuthRibbonNodeMap()
        {
            this.ToTable("AuthRibbonNode");
            this.HasKey(a => a.Id);

            this.Property(a => a.Caption)
                .HasMaxLength(100)
                .IsUnicode(false);

            this.Property(a => a.DisplayName)
                .HasMaxLength(200)
                .IsUnicode(false);

            this.Property(a => a.LinkName)
                .HasMaxLength(200)
                .IsUnicode(false);

            this.Property(a => a.Image)
                .HasMaxLength(500)
                .IsUnicode(false);

            this.Property(a => a.Color)
                .HasMaxLength(20)
                .IsUnicode(false);
                

            this.HasOptional(a => a.Parent)
                .WithMany(a => a.Children)
                .HasForeignKey(a => a.ParentId);

            //this.HasOptional(a => a.RibbonNodeRoot)
            //    .WithMany(a => a.Id)
            //    .HasForeignKey(a => a.RibbonNodeRootId);
        }
    }
}
