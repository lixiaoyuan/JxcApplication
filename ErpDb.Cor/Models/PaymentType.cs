using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models
{
    public enum PaymentType
    {
        [Display(Description = "现金")]
        Cash,
        [Display(Description = "转账")]
        Transfer
    }
}
