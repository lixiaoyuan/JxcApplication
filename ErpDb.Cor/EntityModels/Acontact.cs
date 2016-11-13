namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Acontact
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Acontact()
        {
            Customer = new HashSet<Customer>();
            Suppliers = new HashSet<Suppliers>();
        }

        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool? Sex { get; set; }

        public bool? MainAcontact { get; set; }

        public string Tel { get; set; }

        public string Phone { get; set; }

        public string Qq { get; set; }

        public string Email { get; set; }

        public string Area { get; set; }

        public string Address { get; set; }

        public bool? Enable { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Suppliers> Suppliers { get; set; }
    }
}
