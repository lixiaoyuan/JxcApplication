using System.Data.Entity.ModelConfiguration;

namespace ApplicationDb.Cor.EntityMapping
{
    public class AuthRoleMap : EntityTypeConfiguration<AuthRole>
    {
        public AuthRoleMap()
        {
            ToTable("AuthRole");
            HasKey(a => a.Id);

            Property(e => e.Code)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(e => e.Name)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(a => a.CreateDate).
                HasColumnType("date");

            Property(e => e.CreateUserName)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(e => e.ModifyUserName)
                .IsUnicode(false)
                .HasMaxLength(50);

            HasMany(e => e.AuthRoleMenu)
                .WithOptional(e => e.AuthRole)
                .HasForeignKey(e => e.RoleId);

            HasMany(e => e.AuthUserRole)
                .WithOptional(e => e.AuthRole)
                .HasForeignKey(e => e.RoleId);
        }
    }
}