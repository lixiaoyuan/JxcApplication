using System.ComponentModel.DataAnnotations;

namespace ApplicationDb.Cor.Model
{
    public enum WorkApprovalOrderResult
    {
        /// <summary>
        /// 审批中
        /// </summary>
        [Display(Description = "审批中")]
        Approvaling,
        /// <summary>
        /// 同意
        /// </summary>
        [Display(Description = "同意")]
        Agree,
        /// <summary>
        /// 拒绝
        /// </summary>
        [Display(Description = "拒绝")]
        Refuse,
        /// <summary>
        /// 撤销
        /// </summary>
        [Display(Description = "撤销")]
        Goback
    }
}