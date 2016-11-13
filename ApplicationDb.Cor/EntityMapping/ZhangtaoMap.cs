using System.Data.Entity.ModelConfiguration;

namespace ApplicationDb.Cor.EntityMapping
{
    public class ZhangtaoMap : EntityTypeConfiguration<Zhangtao>
    {
        public ZhangtaoMap()
        {
            ToTable("Zhangtao");
            HasKey(a => a.Id);


            Property(e => e.Name)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(e => e.ConnectionString)
                .IsUnicode(false)
                .HasMaxLength(500);
        }
    }
}