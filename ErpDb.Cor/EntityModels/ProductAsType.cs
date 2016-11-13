using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.EntityModels
{
    public class ProductAsType
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public bool Enable { get; set; }
    }
}
