namespace ApplicationDb.Cor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AuthRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AuthRole()
        {
            AuthRoleMenu = new HashSet<AuthRoleMenu>();
            AuthUserRole = new HashSet<AuthUserRole>();
        }

        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool? Enable { get; set; }

        public DateTime? CreateDate { get; set; }

        public Guid? CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public DateTime? ModifyDate { get; set; }

        public Guid? ModifyUserId { get; set; }

        public string ModifyUserName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuthRoleMenu> AuthRoleMenu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuthUserRole> AuthUserRole { get; set; }
    }
}
