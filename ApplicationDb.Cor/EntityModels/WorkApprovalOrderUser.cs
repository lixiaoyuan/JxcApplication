﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationDb.Cor.Model;

namespace ApplicationDb.Cor.EntityModels
{
    /// <summary>
    /// 用户审批记录
    /// </summary>
    public class WorkApprovalOrderUser
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
        /// 审批用户
        /// </summary>
        public Guid? UserId { get; set; }
        /// <summary>
        /// 审批结果
        /// </summary>
        public string ResultType { get; set; }
        /// <summary>
        /// 评论
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModiftyTime { get; set; }

        public int Sort { get; set; }
        public virtual WorkApprovalOrder WorkApprovalOrder { get; set; }
        public virtual SystemUser ApprovalUser { get; set; }
    }
}
