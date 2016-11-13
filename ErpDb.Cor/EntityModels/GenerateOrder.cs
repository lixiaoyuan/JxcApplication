namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GenerateOrder
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public DateTime? GenerateTime { get; set; }

        public int? Order { get; set; }
    }
}
