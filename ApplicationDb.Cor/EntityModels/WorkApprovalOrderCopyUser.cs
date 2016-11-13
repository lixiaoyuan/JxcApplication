using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.Model;

namespace ApplicationDb.Cor.EntityModels
{
    /// <summary>
    /// 审批记录抄送表
    /// </summary>
    public class WorkApprovalOrderCopyUser
    {
        /// <summary>
        /// 记录Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 审批记录Id
        /// </summary>
        public Guid? WorkApprovalOrderId { get; set; }
        /// <summary>
        /// 抄送用户
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 是否查看
        /// </summary>
        public bool? IsSeed { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModiftyTime { get; set; }

        public virtual WorkApprovalOrder WorkApprovalOrder { get; set; }
        public virtual SystemUser CopyUser{ get; set; }

    }
}
