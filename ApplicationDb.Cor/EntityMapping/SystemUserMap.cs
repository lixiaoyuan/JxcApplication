using System.Data.Entity.ModelConfiguration;

namespace ApplicationDb.Cor.EntityMapping
{
    public class SystemUserMap : EntityTypeConfiguration<SystemUser>
    {
        public SystemUserMap()
        {
            ToTable("SystemUser");
            HasKey(a => a.Id);


            Property(e => e.Code)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(e => e.Password)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(e => e.Salt)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(e => e.Name)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(e => e.Spell)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(e => e.Tel)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(e => e.Email)
                .IsUnicode(false).HasMaxLength(50);

            Property(e => e.IdCard)
                .IsUnicode(false)
                .HasMaxLength(50);

            Property(e => e.Education)
                .IsUnicode(false)
                .HasMaxLength(100);

            Property(e => e.School)
                .IsUnicode(false)
                .HasMaxLength(100);

            Property(e => e.Address)
                .IsUnicode(false)
                .HasMaxLength(300);

            Property(e => e.Remark)
                .IsUnicode(false)
                .HasMaxLength(200);

            Property(e => e.CreateUserName)
                .IsUnicode(false).HasMaxLength(50);

            Property(e => e.ModifyUserName)
                .IsUnicode(false).HasMaxLength(50);

            HasMany(e => e.AuthUserRole)
                .WithOptional(e => e.SystemUser)
                .HasForeignKey(e => e.UserId);
        }
    }
}