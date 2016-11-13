namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductOutStorageDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductOutStorageDetail()
        {
            ProductOutInStorageDetail = new HashSet<ProductOutInStorageDetail>();
        }

        public Guid Id { get; set; }

        public string OrderType { get; set; }

        public Guid? PutOutId { get; set; }

        public Guid? ProductId { get; set; }

        public string ProductCode { get; set; }

        public string ProductSpecification { get; set; }

        public string ProductUnit { get; set; }

        public decimal OutStock { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SumPrice { get; set; }
        public int SortId { get; set; }
        public virtual Product Product { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOutInStorageDetail> ProductOutInStorageDetail { get; set; }

        public virtual ProductOutStorage ProductOutStorage { get; set; }
    }
}
