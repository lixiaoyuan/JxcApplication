using System.Data.Entity.ModelConfiguration;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class ProductAsTypeMap : EntityTypeConfiguration<ProductAsType>
    {
        public ProductAsTypeMap()
        {
            ToTable("ProductAsType");
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