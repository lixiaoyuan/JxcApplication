using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.Model;

namespace ApplicationDb.Cor.EntityModels
{
    /// <summary>
    /// 审批记录
    /// </summary>
    public class WorkApprovalOrder
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 审批类型，外键Id
        /// </summary>
        public Guid? WorkApprovalId { get; set; }
        /// <summary>
        /// 提交人Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 提交人姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public string OrderStatusType { get; set; }
        /// <summary>
        /// 审批结果
        /// </summary>
        public string OrderResultType { get; set; }
        /// <summary>
        /// 发起时间
        /// </summary>
        public DateTime StartingTime { get; set; }
        /// <summary>
        /// 停止时间
        /// </summary>
        public DateTime? StopTime { get; set; } 
        /// <summary>
        /// 下一位审批人
        /// </summary>
        public Guid? NextApprovalUserId { get; set; }
        /// <summary>
        /// 评论(xml)
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 提交单数据
        /// </summary>
        public byte[] FormData { get; set; }

        public virtual WorkApproval WorkApproval { get; set; }
    }
}
