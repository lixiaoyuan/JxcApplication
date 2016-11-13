using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.EntityModels
{
    public class CostType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool? Enable { get; set; }
    }
}
