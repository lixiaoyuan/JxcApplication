using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.ReportDataModel
{
    public class RawOutStoreData
    {
        public class Head
        {
            public DateTime CreateDate { get; set; }
            public string Code { get; set; }
            public string Store { get; set; }
            public string Remark { get; set; }
            public string CreateUser { get; set; }

        }
        public class Detail
        {
            public string Name { get; set; }
            public string ProductSpecification { get; set; }
            public string ProductUnit { get; set; }
            public decimal SubtractStock { get; set; }
            public string StoreLocation { get; set; }
            public string StoreLocationCode { get; set; }
            public DateTime ProducingDate { get; set; }

        }

        public Head Title { get; set; }
        public IEnumerable<Detail> Details { get; set; }
    }
}
