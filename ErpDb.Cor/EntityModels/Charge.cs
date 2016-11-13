namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Charge")]
    public partial class Charge
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Charge()
        {
            ChargeDetails = new HashSet<ChargeDetails>();
        }

        public Guid Id { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? PaymentAccountId { get; set; }

        [Column(TypeName = "money")]
        public decimal? SumPrice { get; set; }

        public short? Discount { get; set; }

        [Column(TypeName = "money")]
        public decimal? DiscountPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal? RealPrice { get; set; }

        public Guid? BusinessUser { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        public Guid? CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(20)]
        public string CreateIp { get; set; }

        public DateTime? ModiftyDate { get; set; }

        [StringLength(20)]
        public string ModiftyIp { get; set; }

        public Guid? ModiftyUserId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChargeDetails> ChargeDetails { get; set; }
    }
}
