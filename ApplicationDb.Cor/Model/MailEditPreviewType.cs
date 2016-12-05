using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ApplicationDb.Cor.Model
{
    public enum MailEditPreviewType
    {
        [Display(Description = "Doc")]
        Doc,
        [Display(Description = "Excel")]
        Excel,
        Null
    }
}
