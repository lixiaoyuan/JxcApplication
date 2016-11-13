namespace ApplicationDb.Cor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Zhangtao
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ConnectionString { get; set; }
    }
}
