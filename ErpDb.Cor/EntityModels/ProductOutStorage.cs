namespace BusinessDb.Cor.EntityModels
{
    using BusinessDb.Cor.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductOutStorage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductOutStorage()
        {
            ProductOutStorageDetail = new HashSet<ProductOutStorageDetail>();
        }

        public Guid Id { get; set; }

        public string OrderType { get; set; }

        public string Code { get; set; }

        public Guid? StorageId { get; set; }

        public DateTime? PaymentDate { get; set; }

        public Guid? BusinessUser { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? AcontackId { get; set; }

        /// <summary>
        /// 送货地址
        /// </summary>
        public string GiveAddress { get; set; }

        /// <summary>
        /// 送货区域
        /// </summary>
        public string GiveArea { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string AcontackName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string AcontackTel { get; set; }

        public decimal? SumPrice { get; set; }

        public decimal? Paid { get; set; }

        public string Remark { get; set; }

        public int? StatusFlag { get; set; }

        /// <summary>
        /// 付款类型
        /// </summary>
        public int PaymoneyType { get; set; }

        /// <summary>
        /// 客户类型
        /// </summary>
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        public Guid? CreateUserId { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreateIp { get; set; }

        public Guid? ModiftyUserId { get; set; }

        public DateTime? ModiftyDate { get; set; }

        public string ModiftyIp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOutStorageDetail> ProductOutStorageDetail { get; set; }
    }
}
