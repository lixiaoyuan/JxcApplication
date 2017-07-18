using System;
using System.Collections.Generic;

namespace BusinessDb.Cor.Models.ReportDataModel
{
    public class OutProductData
    {
        public class Head
        {
            public DateTime CreateDate { get; set; }
            public string Code { get; set; }
            public string GiveAddress { get; set; }
            public string GiveArea { get; set; }
            public string CustomerName { get; set; }
            public string Tel { get; set; }
            public string AcontackName { get; set; }
            public string CreateUser { get; set; }
            public string BusinessUser { get; set; }
            public decimal SumPrice { get; set; }
	        public decimal SumQuant { get; set; }
            public string Remark { get; set; }
            public string PaymoneyType { get; set; }
            public override string ToString()
            {
                return $"Code:{Code} CreateDate:{CreateDate} CreateUser:{CreateUser} GiveAddress:{GiveAddress} GiveArea:{GiveArea} CustomerName:{CustomerName} Tel:{Tel}";

            }
        }
        public class Detail
        {
            public string Name { get; set; }
            public string ProductSpecification { get; set; }
            public string ProductUnit { get; set; }
            public decimal OutStock { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal SumPrice { get; set; }
            public override string ToString()
            {
                return $"Name:{Name} ProductSpecification:{ProductSpecification} ProductUnit:{ProductUnit} OutStock:{OutStock} UnitPrice:{UnitPrice} SumPrice:{SumPrice}";
            }
        }

        public Head Title { get; set; }
        public IEnumerable<Detail> Details { get; set; }

    }
}
