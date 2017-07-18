using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models
{
    /// <summary>
    /// 付款方式
    /// </summary>
    public enum PaymentType
    {
        [Display(Description = "现结")]
        Cash,
        [Display(Description = "转账")]
        Transfer
    }
}
