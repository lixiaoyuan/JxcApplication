namespace ApplicationDb.Cor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AuthMenu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AuthMenu()
        {
            AuthRoleMenu = new HashSet<AuthRoleMenu>();
            AuthMenuToolButton = new HashSet<AuthMenuToolButton>();
            AuthToolButton = new HashSet<AuthToolButton>();
        }

        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string SystemId { get; set; }

        public string DisplayName { get; set; }

        public string BackgroundColor { get; set; }

        public string HeaderTxt { get; set; }

        public bool IsFlowBreak { get; set; }

        public string Size { get; set; }

        public string Window { get; set; }

        public string Image { get; set; }

        public bool? Enable { get; set; }

        public bool? Check { get; set; }

        public bool? IsTile { get; set; }

        public int? TileSort { get; set; }

        public int? MenuSort { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuthRoleMenu> AuthRoleMenu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuthMenuToolButton> AuthMenuToolButton { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuthToolButton> AuthToolButton { get; set; }
    }
}
