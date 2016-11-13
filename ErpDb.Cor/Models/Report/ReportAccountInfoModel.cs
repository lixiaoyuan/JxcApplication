using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.Report
{
    public class ReportAccountInfoModel
    {
        [Display(Name = "编号")]
        public string Code { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "余额")]
        [DataType(DataType.Currency)]
        public decimal? Balance { get; set; }
    }
}
