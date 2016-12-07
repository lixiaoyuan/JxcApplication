using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.Report
{
    /// <summary>
    /// 付款模型
    /// </summary>
    public class PaymentModel
    {
        [Display(Name = "单号")]
        public string Code { get; set; }
        [Display(Name = "订单类型")]
        public string OrderType { get; set; }
        [Display(Name = "客户")]
        public string Customer { get; set; }
        [Display(Name = "客户类型")]
        public string CustomerType { get; set; }
        [Display(Name = "总金额")]
        [DataType(DataType.Currency)]
        public decimal? SumPrice { get; set; }
        [Display(Name = "已付金额")]
        [DataType(DataType.Currency)]
        public decimal? Paid { get; set; }
        [Display(Name = "未付金额")]
        [DataType(DataType.Currency)]
        public decimal? NoPayment { get; set; }
        [Display(Name = "业务员")]
        public string BusinessUser { get; set; }
        [Display(Name = "开单员")]
        public string CreateUser { get; set; }
        [Display(Name = "开单日期")]
        public DateTime? CreateDate { get; set; }
    }
}
