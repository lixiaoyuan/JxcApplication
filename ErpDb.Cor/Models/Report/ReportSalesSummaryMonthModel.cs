using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.Report
{
    public class ReportSalesSummaryMonthModel
    {
        [Display(Name = "客户")]
        public string CustomerName { get; set; }
        [Display(Name = "业务员")]
        public string ResponsibleName { get; set; }
        [Display(Name = "类型")]
        public string CustomerType { get; set; }
        [Display(Name = "一月")]
        public decimal? January { get; set; }
        [Display(Name = "二月")]
        public decimal? February { get; set; }
        [Display(Name = "三月")]
        public decimal? March { get; set; }
        [Display(Name = "四月")]
        public decimal? April { get; set; }
        [Display(Name = "五月")]
        public decimal? May { get; set; }
        [Display(Name = "六月")]
        public decimal? June { get; set; }
        [Display(Name = "七月")]
        public decimal? July { get; set; }
        [Display(Name = "八月")]
        public decimal? August { get; set; }
        [Display(Name = "九月")]
        public decimal? September { get; set; }
        [Display(Name = "十月")]
        public decimal? October { get; set; }
        [Display(Name = "十一月")]
        public decimal? November { get; set; }
        [Display(Name = "十二月")]
        public decimal? December { get; set; }
        [Display(Name = "合计")]
        public decimal? SumMoney { get; set; }
    }
}
