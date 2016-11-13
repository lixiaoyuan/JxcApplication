namespace ApplicationDb.Cor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SystemProgram
    {
        public Guid Id { get; set; }

        public string SystemId { get; set; }

        public bool? Enable { get; set; }
    }
}
