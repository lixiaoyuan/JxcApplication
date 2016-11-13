using BusinessDb.Cor.Models;

namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CustomerAccountRecord
    {
        public Guid Id { get; set; }

        public Guid? CustomerId { get; set; }

        public BusinessDb.Cor.Models.TransactionTypeEnum? TransactionType { get; set; }

        public decimal? TransactionBefore { get; set; }

        public decimal? TransactionBalance { get; set; }

        public decimal? TransactionAfter { get; set; }

        public Guid? CreateUserId { get; set; }

        public DateTime? CreateDate { get; set; }

        public string CreateIp { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
