namespace ApplicationDb.Cor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AuthToolButton
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AuthToolButton()
        {
            AuthMenuToolButton = new HashSet<AuthMenuToolButton>();
        }

        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Name { get; set; }

        public Guid? MenuId { get; set; }

        public string SystemId { get; set; }

        public bool? Root { get; set; }

        public string LinkType { get; set; }

        public string LinkName { get; set; }

        public int? SortCode { get; set; }

        public bool? Enable { get; set; }

        public bool? Check { get; set; }

        public virtual AuthMenu AuthMenu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AuthMenuToolButton> AuthMenuToolButton { get; set; }
    }
}
