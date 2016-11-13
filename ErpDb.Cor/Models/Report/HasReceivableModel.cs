using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.Report
{
    /// <summary>
    /// 收款模型
    /// </summary>
    public class HasReceivableModel
    {
        [Display(Name = "单号")]
        public string Code { get; set; }
        [Display(Name = "客户编码")]
        public string CustomerCode { get; set; }
        [Display(Name = "客户")]
        public string Customer { get; set; }
        [Display(Name = "账户")]
        public string Account { get; set; }
        [Display(Name = "总金额")]
        public decimal SumPrice { get; set; }
        [Display(Name = "收款员")]
        public string CreateUser { get; set; }
        [Display(Name = "开单日期")]
        public DateTime CreateDate { get; set; }
    }
}
