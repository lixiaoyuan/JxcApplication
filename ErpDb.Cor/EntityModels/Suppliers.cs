namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Suppliers
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string AllName { get; set; }

        public string ZipCode { get; set; }

        public string Fax { get; set; }

        public Guid? AContactId { get; set; }

        public string AContact { get; set; }

        public string AContactPhone { get; set; }

        public string AContactQq { get; set; }

        public string AContactEmail { get; set; }

        public string AContactAddress { get; set; }

        public bool? Enable { get; set; }

        public virtual Acontact Acontact1 { get; set; }
    }
}
