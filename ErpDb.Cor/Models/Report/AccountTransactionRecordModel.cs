using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models.Report
{
    public class AccountTransactionRecordModel
    {
        [Display(Name = "交易时间")]
        public DateTime? CreateDate { get; set; }
        [Display(Name = "账户")]
        public string Account { get; set; }
        [Display(Name = "交易类型")]
        public string TransactionType { get; set; }
        [Display(Name = "期初金额")]
        [DataType(DataType.Currency)]
        public decimal? TransactionBefore { get; set; }
        [Display(Name = "交易金额")]
        [DataType(DataType.Currency)]
        public decimal? TransactionBalance { get; set; }
        [Display(Name = "期末金额")]
        [DataType(DataType.Currency)]
        public decimal? TransactionAfter { get; set; }
        [Display(Name = "操作员")]
        public string CreateUser { get; set; }
    }
}
