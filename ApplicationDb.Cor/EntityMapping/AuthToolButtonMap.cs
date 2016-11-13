using System.Data.Entity.ModelConfiguration;

namespace ApplicationDb.Cor.EntityMapping
{
    public class AuthToolButtonMap : EntityTypeConfiguration<AuthToolButton>
    {
        public AuthToolButtonMap()
        {
            ToTable("AuthToolButton");
            HasKey(a => a.Id);


            Property(e => e.Name)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(e => e.SystemId)
                .IsUnicode(false)
                .HasMaxLength(20);

            Property(e => e.LinkType)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(e => e.LinkName)
                .IsUnicode(false)
                .HasMaxLength(50);

            HasMany(e => e.AuthMenuToolButton)
                .WithOptional(e => e.AuthToolButton)
                .HasForeignKey(e => e.ToolButtonId);
        }
    }
}