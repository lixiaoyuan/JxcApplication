namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductOutInStorageDetail
    {
        public Guid Id { get; set; }

        public Guid? PutInStorageId { get; set; }

        public Guid? PutOutStorageId { get; set; }

        public decimal? OriginalStock { get; set; }

        public decimal? SubtractStock { get; set; }

        public decimal? LasetStock { get; set; }

        public DateTime? CreateDate { get; set; }

        public Guid? CreateUserId { get; set; }

        public string CreateIp { get; set; }

        public virtual ProductInStorageDetail ProductInStorageDetail { get; set; }

        public virtual ProductOutStorageDetail ProductOutStorageDetail { get; set; }
    }
}
