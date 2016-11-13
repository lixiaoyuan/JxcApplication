using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.Report
{
    public class ProductStock
    {
        [Display(Name = "产品编码")]
        public string Code { get; set; }
        [Display(Name = "产品名称")]
        public string ProductName { get; set; }
        [Display(Name = "原库存",AutoGenerateField = false)]
        public decimal OriginalStock { get; set; }
        [Display(Name = "规格")]
        public string Specification { get; set; }
        [Display(Name = "类别")]
        public string ProductAsType { get; set; }
        [Display(Name = "单位")]
        public string Unit { get; set; }
        [Display(Name = "剩余库存")]
        public decimal LastStock { get; set; }
    }
}
