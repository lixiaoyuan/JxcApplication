namespace BusinessDb.Cor.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductType
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        /// <summary>
        /// 是否可以销售
        /// </summary>
        public bool IsSell { get; set; }
    }
}
