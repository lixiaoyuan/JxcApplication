namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderType
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ReportUri { get; set; }

        public string Remark { get; set; }
    }
}
