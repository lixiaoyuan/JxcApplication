using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models
{
    public enum OrderType : short
    {
        [Display(Description = "销售退单")]
        XT,
        [Display(Description = "销售开单")]
        XK,
        [Display(Description = "清零单")]
        QL,
        [Display(Description = "成品入库单")]
        CR,
        [Display(Description = "消耗品领用单")]
        XL,
        [Display(Description = "消耗品入库单")]
        XR
    }
}
