using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.Report
{
    public class StockAlarmModel
    {
        [Display(Name = "仓库")]
        public string Storage { get; set; }
        [Display(Name = "产品")]
        public string Product { get; set; }
        [Display(Name = "产品规格")]
        public string ProductSpecification { get; set; }
        [Display(Name = "产品分类")]
        public string ProductAsType { get; set; }
        [Display(Name = "剩余库存")]
        public decimal? LastStock { get; set; }
        [Display(Name = "提醒库存")]
        public decimal? StockRemind { get; set; }
    }
}
