using System;
using System.ComponentModel.DataAnnotations;

namespace ApplicationDb.Cor.Model
{
    
    public enum WorkApprovalOrderStatus
    {
        /// <summary>
        /// 审批中
        /// </summary>
        [Display(Description = "审批中")]
        Underway,
        /// <summary>
        /// 审批完成
        /// </summary>
        [Display(Description = "审批完成")]
        Completed,
        /// <summary>
        /// 撤回
        /// </summary>
        [Display(Description = "撤回")]
        Revoke
    }
}