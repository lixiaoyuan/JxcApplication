using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.EntityModels
{
    public class ExpensesDetail
    {
        public Guid Id { get; set; }
        public Guid? ExpensesId { get; set; }
        public Guid CostTypeId { get; set; }
        public string Mark { get; set; }
        public decimal? Cost { get; set; }
        public virtual Expenses Expenses { get; set; }
    }
}
