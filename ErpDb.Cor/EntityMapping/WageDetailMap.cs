using System.Data.Entity.ModelConfiguration;
using BusinessDb.Cor.EntityModels;

namespace BusinessDb.Cor.EntityMapping
{
    public class WageDetailMap : EntityTypeConfiguration<WageDetail>
    {
        public WageDetailMap()
        {
            ToTable("WageDetail");
            HasKey(a => a.Id);

            Property(a => a.Name)
                .IsUnicode(false)
                .HasMaxLength(20);

            Property(a => a.WorkDay)
                .HasPrecision(4, 2);

            Property(a => a.C1)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C2)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C3)
                .HasPrecision(10, 2)
                .HasColumnType("money");

            Property(a => a.C4)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C5)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C6)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C7)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C8)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C9)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C10)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C11)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C12)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C13)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C14)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C15)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C16)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.X1)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.X2)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.X3)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.PreTaxSum)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.C17)
                .HasPrecision(10, 2)
                .HasColumnType("money");
            Property(a => a.AfterTaxSum)
                .HasPrecision(10, 2)
                .HasColumnType("money");
        }
    }
}