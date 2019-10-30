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
        /// �ͻ���ַ
        /// </summary>
        public string GiveAddress { get; set; }

        /// <summary>
        /// �ͻ�����
        /// </summary>
        public string GiveArea { get; set; }

        /// <summary>
        /// ��ϵ������
        /// </summary>
        public string AcontackName { get; set; }

        /// <summary>
        /// ��ϵ�绰
        /// </summary>
        public string AcontackTel { get; set; }

        public decimal? SumPrice { get; set; }

        public decimal? Paid { get; set; }

        public string Remark { get; set; }

        public int? StatusFlag { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public int PaymoneyType { get; set; }

        /// <summary>
        /// �ͻ�����
        /// </summary>
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// �ͻ�����
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
