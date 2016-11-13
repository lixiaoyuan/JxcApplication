using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JxcApplication.Core.WorkApprovalData
{
    /// <summary>
    /// 请假
    /// </summary>
    [Serializable]
    public class ForleaveData
    {
        public string ForleaveType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string LeaveDayTimes { get; set; }
        public string Reason { get; set; }
    }
}
