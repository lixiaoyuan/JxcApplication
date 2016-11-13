using System;
using System.Collections.Generic;

namespace BusinessDb.Cor.Models.ReportDataModel
{
    public class ProductInStroeData
    {
        public class Head
        {
            public DateTime CreateDate { get; set; }
            public string Code { get; set; }
            public string Stroe { get; set; }
            public DateTime ProducingDate { get; set; }
            public string Remark { get; set; }
            public string CreateUser { get; set; }
            public string BusinessUser { get; set; }
             
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
