using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.EntityMapping
{
    public class SystemProgramMap:EntityTypeConfiguration<SystemProgram>
    {
        public SystemProgramMap()
        {
            this.ToTable("SystemProgram");
            this.HasKey(a => a.Id);

           Property(e => e.SystemId)
                .IsUnicode(false)
                .HasMaxLength(20);
        }
    }
}
