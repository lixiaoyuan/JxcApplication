namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductInStorageDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductInStorageDetail()
        {
            ProductOutInStorageDetail = new HashSet<ProductOutInStorageDetail>();
        }

        public Guid Id { get; set; }

        public string OrderType { get; set; }

        public Guid? PutInId { get; set; }

        public Guid? ReturnInId { get; set; }

        public Guid? StorageId { get; set; }

        public Guid? ProductId { get; set; }

        public string ProductCode { get; set; }

        public string ProductSpecification { get; set; }

        public string ProductUnit { get; set; }

        public decimal OriginalStock { get; set; }

        public decimal LastStock { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SumPrice { get; set; }

        public short SortCode { get; set; }

        public string Remark { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductInStorage ProductInStorage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOutInStorageDetail> ProductOutInStorageDetail { get; set; }

        public virtual ProductReturnInStorage ProductReturnInStorage { get; set; }
    }
}
