using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.EntityMapping
{
    public class AuthRoleMenuMap:EntityTypeConfiguration<AuthRoleMenu>
    {
        public AuthRoleMenuMap()
        {
            this.ToTable("AuthRoleMenu");
            this.HasKey(a => a.Id);

            this.Property(a => a.CreateUserName)
                .HasMaxLength(50).IsUnicode(false);
        }
    }
}
