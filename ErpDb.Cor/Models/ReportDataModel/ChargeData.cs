using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.ReportDataModel
{
    public class ChargeData
    {
        public Head Title { get; set; }
        public IEnumerable<Detail> Details { get; set; }

        public class Head
        {
            public string Code { get; set; }
            public string Customer { get; set; }
            public string PayAccount { get; set; }
            public decimal SumPrice { get; set; }
            public string BusinessUser { get; set; }
            public string Remark { get; set; }
            public string CreateUser { get; set; }
            public DateTime CreateDate { get; set; }
        }

       public class Detail
       {
           public string OrderType { get; set; }
           public string RefCode { get; set; }
           public DateTime RefDate { get; set; }
           public decimal SumPrice { get; set; }
           public decimal Paid { get; set; }
           public decimal ThisPrice { get; set; }
           public string Remark { get; set; }
       }
    }
}
