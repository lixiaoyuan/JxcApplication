namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public  class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            AccountRecord = new HashSet<AccountRecord>();
        }

        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal? Balance { get; set; }

        public Guid? CreateUserId { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreateIp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountRecord> AccountRecord { get; set; }
    }
}
