using System.Data.Entity.ModelConfiguration;
using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor.EntityMapping
{
    public class ForleaveTypeMap : EntityTypeConfiguration<ForleaveType>
    {
        public ForleaveTypeMap()
        {
            ToTable("ForleaveType");
            HasKey(a => a.Id);

            Property(a => a.Code)
                .IsUnicode(false)
                .HasMaxLength(30);

            Property(a => a.Name)
                .IsUnicode(false)
                .HasMaxLength(100);
        }
    }
}