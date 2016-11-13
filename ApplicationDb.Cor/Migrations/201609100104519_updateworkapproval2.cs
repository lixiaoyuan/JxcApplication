namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateworkapproval2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.WorkApprovalCopyUser", name: "UserId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.WorkApprovalCopyUser", name: "ApprovalId", newName: "UserId");
            RenameColumn(table: "dbo.WorkApprovalUser", name: "UserId", newName: "__mig_tmp__1");
            RenameColumn(table: "dbo.WorkApprovalUser", name: "ApprovalId", newName: "UserId");
            RenameColumn(table: "dbo.WorkApprovalCopyUser", name: "__mig_tmp__0", newName: "ApprovalId");
            RenameColumn(table: "dbo.WorkApprovalUser", name: "__mig_tmp__1", newName: "ApprovalId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.WorkApprovalUser", name: "ApprovalId", newName: "__mig_tmp__1");
            RenameColumn(table: "dbo.WorkApprovalCopyUser", name: "ApprovalId", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.WorkApprovalUser", name: "UserId", newName: "ApprovalId");
            RenameColumn(table: "dbo.WorkApprovalUser", name: "__mig_tmp__1", newName: "UserId");
            RenameColumn(table: "dbo.WorkApprovalCopyUser", name: "UserId", newName: "ApprovalId");
            RenameColumn(table: "dbo.WorkApprovalCopyUser", name: "__mig_tmp__0", newName: "UserId");
        }
    }
}
