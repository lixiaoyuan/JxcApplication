using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.EntityModels
{
    /// <summary>
    /// 请假类型
    /// </summary>
    public class ForleaveType
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public bool Enable { get; set; }
    }
}
