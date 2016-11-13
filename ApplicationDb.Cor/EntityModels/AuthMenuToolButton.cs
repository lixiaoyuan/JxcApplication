namespace ApplicationDb.Cor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AuthMenuToolButton
    {
        public Guid Id { get; set; }

        public Guid? RoleId { get; set; }

        public Guid? MenuId { get; set; }

        public Guid? ToolButtonId { get; set; }

        public DateTime? CreateDate { get; set; }

        public Guid? CreateUserId { get; set; }

        public string CreateUserName { get; set; }

        public virtual AuthMenu AuthMenu { get; set; }

        public virtual AuthRole AuthRole { get; set; }

        public virtual AuthToolButton AuthToolButton { get; set; }
    }
}
