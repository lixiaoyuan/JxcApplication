using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.Report
{
    public class ExpensesModel
    {
        [Display(Name = "订单号")]
        public string Code { get; set; }
        [Display(Name = "订单类型")]
        public string OrderType { get; set; }
        [Display(Name = "录入时间")]
        public DateTime CreteDate { get; set; }
        [Display(Name = "录入员")]
        public string CreateUser { get; set; }
        [Display(Name = "交易账户")]
        public string Account { get; set; }
        [Display(Name = "费用类别")]
        public string CostType { get; set; }
        [Display(Name = "费用金额")]
        public decimal? Cost { get; set; }
        [Display(Name = "备注")]
        public string Mark { get; set; }
        [Display(Name = "票号")]
        public string Bill { get; set; }
        [Display(Name = "序号")]
        public int Order { get; set; }
        
    }
}
