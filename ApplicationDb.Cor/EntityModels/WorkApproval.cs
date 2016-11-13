using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.EntityModels
{
    
    /// <summary>
    /// 审批
    /// </summary>
    public class WorkApproval
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
        public string Remark { get; set; }
        public string FormDataTemplate { get; set; }
        public string DataType { get; set; }
    }
}
