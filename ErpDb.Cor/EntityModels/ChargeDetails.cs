namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ChargeDetails
    {
        public Guid Id { get; set; }

        public Guid? ChargeId { get; set; }

        public string OrderType { get; set; }

        public Guid? RefId { get; set; }

        public string RefCode { get; set; }

        public DateTime? RefDate { get; set; }

        public decimal? SumPrice { get; set; }

        public decimal? Paid { get; set; }

        public decimal? LastPrice { get; set; }

        public decimal? ThisPrice { get; set; }

        public string Remark { get; set; }

        public virtual Charge Charge { get; set; }
    }
}
