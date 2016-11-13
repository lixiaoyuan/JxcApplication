using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.EntityMapping
{
    public class AuthUserRoleMap:EntityTypeConfiguration<AuthUserRole>
    {
        public AuthUserRoleMap()
        {
            this.ToTable("AuthUserRole");
            this.HasKey(a => a.Id);


            Property(e => e.CreateUserName)
                .IsUnicode(false)
                .HasMaxLength(50);
        }
    }
}
