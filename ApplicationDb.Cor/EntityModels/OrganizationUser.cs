using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.EntityModels
{
   public class OrganizationUser
    {
       public Guid Id { get; set; }
       public Guid OrganizationId { get; set; }
       public Guid UserId { get; set; }
    }
}
