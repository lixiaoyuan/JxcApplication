using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.Report
{
    public class ProductInStoreModel
    {
        [Display(Name = "仓库")]
        public string Store { get; set; }
        [Display(Name = "订单类型")]
        public string OrderType { get; set; }
        [Display(Name = "订单号")]
        public string Code { get; set; }
        [Display(Name = "开单日期")]
        public DateTime? CreteDate { get; set; }
        [Display(Name = "产品编码")]
        public string ProductCode { get; set; }
        [Display(Name = "产品")]
        public string Product { get; set; }
        [Display(Name = "产品规格")]
        public string ProductSpecification { get; set; }
        [Display(Name = "产品单位")]
        public string ProductUnit { get; set; }
        [Display(Name = "入库数量")]
        public decimal? OriginalStock { get; set; }
        [Display(Name = "总金额")]
        [DataType(DataType.Currency)]
        public decimal? SumPrice { get; set; }
        [Display(Name = "操作员")]
        public string CreateUser { get; set; }
    }
}
