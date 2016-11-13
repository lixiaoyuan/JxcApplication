using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxcApplication.Core.WorkApprovalData
{
    /// <summary>
    /// 报销
    /// </summary>
    [Serializable]
    public class ReimburseData
    {
        public decimal Money { get; set; }
        public string ReimburseType { get; set; }
        public string ChargeDetails { get; set; }
    }
}
