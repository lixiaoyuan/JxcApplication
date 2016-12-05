using ApplicationDb.Cor.EntityModels;

namespace ApplicationDb.Cor
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base(ApplicationDb.Cor.Helper.ConnectStringHelper.ConnectString)
        {
        }

        public virtual DbSet<AuthMenu> AuthMenu { get; set; }
        public virtual DbSet<AuthMenuToolButton> AuthMenuToolButton { get; set; }
        public virtual DbSet<AuthRole> AuthRole { get; set; }
        public virtual DbSet<AuthRoleMenu> AuthRoleMenu { get; set; }
        public virtual DbSet<AuthToolButton> AuthToolButton { get; set; }
        public virtual DbSet<AuthUserRole> AuthUserRole { get; set; }
        public virtual DbSet<SystemProgram> SystemProgram { get; set; }
        public virtual DbSet<SystemUser> SystemUser { get; set; }
        public virtual DbSet<Zhangtao> Zhangtao { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<AuthRibbonNode> AuthRibbonNode { get; set; }

        public virtual DbSet<WorkApproval> WorkApproval { get; set; }
        public virtual DbSet<WorkApprovalUser> WorkApprovalUser { get; set; }
        public virtual DbSet<WorkApprovalCopyUser> WorkApprovalCopyUser { get; set; }
        public virtual DbSet<WorkApprovalOrder> WorkApprovalOrder { get; set; }
        public virtual DbSet<WorkApprovalOrderUser> WorkApprovalOrderUser { get; set; }
        public virtual DbSet<WorkApprovalOrderCopyUser> WorkApprovalOrderCopyUser { get; set; }
        public virtual DbSet<ForleaveType> ForleaveType { get; set; }
        public virtual DbSet<FileCabinets> FileCabinetses { get; set; }
        public virtual DbSet<MailOrder> MailOrders { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(this.GetType().Assembly);
        }
    }
}
