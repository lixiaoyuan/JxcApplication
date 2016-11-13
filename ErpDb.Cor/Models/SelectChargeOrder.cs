using System;

namespace BusinessDb.Cor.Models
{
    public class SelectChargeOrder
    {
        public Nullable<Guid> Id { get; set; }
        public string OrderType { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<decimal> SumPrice { get; set; }
        public Nullable<decimal> Paid { get; set; }
        public Nullable<decimal> UnPay { get; set; }
        public string Remark { get; set; }
    }
}
