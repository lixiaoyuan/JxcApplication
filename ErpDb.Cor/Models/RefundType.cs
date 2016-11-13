using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.Models
{
    public enum RefundType : short
    {
        /// <summary>
        /// 冲抵往来
        /// </summary>
        [Display(Description = "冲抵往来")]
        OffsetTransactions,
        /// <summary>
        /// 退还现款
        /// </summary>
        [Display(Description = "退还现款")]
        RefundCash,
        /// <summary>
        /// 返利，冲抵往来返回金额，库存不增加
        /// </summary>
        [Display(Description = "返  利")]
        Rebate
    }
}
