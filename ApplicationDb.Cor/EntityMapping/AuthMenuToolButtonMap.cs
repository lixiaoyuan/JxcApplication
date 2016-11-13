using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.EntityMapping
{
    public class AuthMenuToolButtonMap:EntityTypeConfiguration<AuthMenuToolButton>
    {
        public AuthMenuToolButtonMap()
        {
            this.ToTable("AuthMenuToolButton");

            this.HasKey(a => a.Id);

            this.Property(a => a.CreateUserName).HasMaxLength(50).IsUnicode(false);
        }
    }
}
