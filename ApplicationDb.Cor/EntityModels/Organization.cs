using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.EntityModels
{
    public class Organization
    {
        public Guid Id { get; set; }
        public Nullable<Guid> ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public bool Enable { get; set; }
        public bool Checked { get; set; }
        public virtual Organization Parent { get; set; }
        public virtual ICollection<Organization> Children { get; set; }
    }
}
