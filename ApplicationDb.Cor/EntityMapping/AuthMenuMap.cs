using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.EntityMapping
{
    public class AuthMenuMap: EntityTypeConfiguration<AuthMenu>
    {
        public AuthMenuMap()
        {
            this.ToTable("AuthMenu");
            this.HasKey(a => a.Id);

            this.Property(a => a.SystemId)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(a => a.DisplayName)
                .HasMaxLength(50)
                .IsUnicode(false);

            this.Property(e => e.BackgroundColor)
                .HasMaxLength(15)
                .IsUnicode(false);

            this.Property(a => a.HeaderTxt)
                .HasMaxLength(50)
                .IsUnicode(false);

            this.Property(a => a.Size)
                .HasMaxLength(20)
                .IsUnicode(false);

            this.Property(a => a.Window)
                .HasMaxLength(200)
                .IsUnicode(false);

            this.Property(a => a.Image)
                .HasMaxLength(300)
                .IsUnicode(false);


            this.HasMany(e => e.AuthRoleMenu)
                .WithOptional(e => e.AuthMenu)
                .HasForeignKey(e => e.MenuId);

            this.HasMany(e => e.AuthMenuToolButton)
                .WithOptional(e => e.AuthMenu)
                .HasForeignKey(e => e.MenuId);

            this.HasMany(e => e.AuthToolButton)
                .WithOptional(e => e.AuthMenu)
                .HasForeignKey(e => e.MenuId);
        }
    }
}
