using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessDb.Cor.EntityModels
{
    public class TransactionType
    {
        public Guid Id { get; set; }
        public short Code { get; set; }
        public string CodeName { get; set; }
        public string Name { get; set; }
    }
}
