using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SystemUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SystemUser()
        {
            AuthUserRole = new HashSet<AuthUserRole>();
        }

        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public string Name { get; set; }

        public string Spell { get; set; }

        public bool? Gender { get; set; }

        public DateTime? Birthday { get; set; }

        public string Tel { get; set; }

        public string Email { get; set; }

        public bool? Enable { get; set; }

        public bool? Check { get; set; }

        public DateTime? ChangePasswordDate { get; set; }

        public DateTime? CreateDate { get; set; }

        public Guid? CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime? ModifyDate { get; set; }

        public Guid? ModifyUserId { get; set; }

        public string ModifyUserName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuthUserRole> AuthUserRole { get; set; }
        
    }
}
