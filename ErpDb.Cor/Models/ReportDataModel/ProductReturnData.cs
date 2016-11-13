using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.ReportDataModel
{
    public class ProductReturnData
    {
        public class Head
        {
            public string Code { get; set; }
            public string Store { get; set; }
            public string Customer { get; set; }
            public string BuinessUser { get; set; }
            public string AcontactUser { get; set; }
            public string PaymentType { get; set; }
            public string PaymentAccount { get; set; }
            public decimal SumPrice { get; set; }
            public string CreateUser { get; set; }
            public DateTime CreateDate { get; set; }
        }

        public class Detail
        {
            public string Name { get; set; }
            public string ProductSpecification { get; set; }
            public string ProductUnit { get; set; }
            public decimal OriginalStock { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal SumPrice { get; set; }

        }

        public Head Title { get; set; }
        public IEnumerable<Detail> Details { get; set; }
    }
}
