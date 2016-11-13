using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.EntityModels
{
    public class Expenses
    {
        public Expenses()
        {
            ExpensesDetail=new HashSet<ExpensesDetail>();
        }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string OrderType { get; set; }
        public Guid? PaymentAccountId { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? Acontact { get; set; }
        /// <summary>
        /// 票号
        /// </summary>
        public string Bill { get; set; }
        public int Order { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public Guid? Attn { get; set; }
        public decimal? SumMoney { get; set; }
        public Guid? CreteUserId { get; set; }

        public DateTime? CreteDate { get; set; }

        public string CreteIp { get; set; }

        public Guid? ModiftyUserId { get; set; }

        public DateTime? ModiftyDate { get; set; }

        public string ModiftyIp { get; set; }

        public virtual ICollection<ExpensesDetail> ExpensesDetail { get; set; }
    }
}
