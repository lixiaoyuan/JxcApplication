using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor.EntityMapping
{
    public class OrganizationUserMap:EntityTypeConfiguration<OrganizationUser>
    {
        public OrganizationUserMap()
        {
            this.ToTable("OrganizationUser");
            this.HasKey(a => a.Id);
        }
    }
}
