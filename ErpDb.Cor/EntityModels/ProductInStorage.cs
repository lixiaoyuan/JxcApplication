namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductInStorage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductInStorage()
        {
            ProductInStorageDetail = new HashSet<ProductInStorageDetail>();
        }

        public Guid Id { get; set; }

        public string OrderType { get; set; }

        public string Code { get; set; }

        public Guid? StorageId { get; set; }

        public DateTime? ProducingDate { get; set; }

        public int? LifeDay { get; set; }

        public string UnitsPurchased { get; set; }

        public DateTime? BuyTime { get; set; }

        public DateTime? WarrantyExpirationTime { get; set; }

        public Guid? Acontact { get; set; }

        public Guid? Suppliers { get; set; }

        public Guid? DeliveredUser { get; set; }

        public string Factory { get; set; }

        public decimal? SumPrice { get; set; }

        public string Remark { get; set; }

        public int? StatusFlag { get; set; }

        public Guid? CreteUserId { get; set; }

        public DateTime? CreteDate { get; set; }

        public string CreteIp { get; set; }

        public Guid? ModiftyUserId { get; set; }

        public DateTime? ModiftyDate { get; set; }

        public string ModiftyIp { get; set; }

        public virtual Store Store { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInStorageDetail> ProductInStorageDetail { get; set; }
    }
}
