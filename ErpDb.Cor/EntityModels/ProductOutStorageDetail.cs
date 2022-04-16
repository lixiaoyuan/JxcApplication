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

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        public string ProductSpecification { get; set; }

        public string ProductUnit { get; set; }

        public decimal OutStock { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SumPrice { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        public int SortId { get; set; }
        
        /// <summary>
        /// 仓库Id，放弃使用主表的仓库id
        /// </summary>
        public Guid? StoreId { get; set; }

        /// <summary>
        /// 所在仓库 备注信息
        /// </summary>
        public string StoreLocation { get; set; }
        /// <summary>
        /// 所在库位  备注信息
        /// </summary>
        public string StoreLocationCode { get; set; }

        public virtual Product Product { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOutInStorageDetail> ProductOutInStorageDetail { get; set; }

        public virtual ProductOutStorage ProductOutStorage { get; set; }
    }
}
