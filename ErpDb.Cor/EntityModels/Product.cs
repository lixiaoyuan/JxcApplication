namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product(){
            ProductInStorageDetail = new HashSet<ProductInStorageDetail>();
            ProductOutStorageDetail = new HashSet<ProductOutStorageDetail>();
        }

        public Guid Id { get; set; }

        public string ProductType { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string PyCode { get; set; }
        /// <summary>
        /// 产品类别
        /// </summary>
        public Guid? ProductAsType { get; set; }
        public string Specification { get; set; }

        public string Unit { get; set; }

        public decimal? UnitPrice { get; set; }

        public decimal? StockRemind { get; set; }

        public Guid? Suppliers { get; set; }

        public string Manufacturer { get; set; }

        public int? LifeDay { get; set; }
        /// <summary>
        /// 锁定数量
        /// </summary>
        public decimal? LockAmount { get; set; }
        public bool? Enable { get; set; }

        /// <summary>
        /// 所在仓库
        /// </summary>
        public string StoreLocation { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string StoreLocationCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductInStorageDetail> ProductInStorageDetail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOutStorageDetail> ProductOutStorageDetail { get; set; }
    }
}
