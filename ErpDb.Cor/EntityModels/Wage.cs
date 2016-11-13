using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.EntityModels
{
    /// <summary>
    /// 工资
    /// </summary>
    public class Wage
    {
        public Wage()
        {
            WageDetail = new HashSet<WageDetail>();
        }
        public Guid Id { get; set; }
        public DateTime WageDate { get; set; }
        public decimal SumPrice { get; set; }
        public Guid PaymentAccountId { get; set; }
        public string Remark { get; set; }
        public Guid? CreateUserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateIp { get; set; }
        public Guid? ModiftyUserId { get; set; }
        public DateTime? ModiftyDate { get; set; }
        public string ModiftyIp { get; set; }
        public virtual ICollection<WageDetail> WageDetail { get; set; }
    }
}
