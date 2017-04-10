using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.Report
{
    public class SalesDetailModelUser
    {
        [Display(Name = "业务员编码")]
        public string Code { get; set; }
        [Display(Name = "业务员")]
        public string Name { get; set; }
        [Display(Name = "客户")]
        public string CustomerName { get; set; }
        [Display(Name = "总金额")]
        [DataType(DataType.Currency)]
        public decimal SumPrice { get; set; }
    }

    public class SalesDetailModelCustomer
    {
        [Display(Name = "客户编码")]
        public string Code { get; set; }
        [Display(Name = "客户")]
        public string Name { get; set; }
        [Display(Name = "业务员")]
        public string BusinessName { get; set; }
        [Display(Name = "总金额")]
        [DataType(DataType.Currency)]
        public decimal SumPrice { get; set; }
    }
    public class SalesDetailModel
    {
        [Display(Name = "客户")]
        public string Customer { get; set; }
        [Display(Name = "客户类型")]
        public string CustomerType { get; set; }
        [Display(Name = "业务员")]
        public string BusinessUser { get; set; }
        [Display(Name = "订单类型")]
        public string OrderType { get; set; }
        [Display(Name = "订单号")]
        public string Code { get; set; }
        [Display(Name = "开单日期")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "产品编码")]
        public string ProductCode { get; set; }
        [Display(Name = "产品名称")]
        public string ProductName { get; set; }
        [Display(Name = "规格")]
        public string ProductSpecification { get; set; }
        [Display(Name = "单位")]
        public string ProductUnit { get; set; }
        [Display(Name = "数量")]
        public decimal? OutStock { get; set; }
        [Display(Name = "单价")]
        [DataType(DataType.Currency)]
        public decimal? UnitPrice { get; set; }
        [Display(Name = "总价")]
        [DataType(DataType.Currency)]
        public decimal? SumPrice { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(AutoGenerateField = false)]
        public Guid PutOutId { get; set; }
    }
}
