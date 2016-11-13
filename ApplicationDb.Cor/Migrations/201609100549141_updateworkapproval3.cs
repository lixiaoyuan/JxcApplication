namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateworkapproval3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.WorkApprovalCopyUser", "ApprovalId", "dbo.WorkApproval");
            DropForeignKey("dbo.WorkApprovalCopyUser", "UserId", "dbo.SystemUser");
            DropIndex("dbo.WorkApprovalCopyUser", new[] { "ApprovalId" });
            DropIndex("dbo.WorkApprovalCopyUser", new[] { "UserId" });
            DropTable("dbo.WorkApprovalCopyUser");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.WorkApprovalCopyUser",
                c => new
                    {
                        ApprovalId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApprovalId, t.UserId });
            
            CreateIndex("dbo.WorkApprovalCopyUser", "UserId");
            CreateIndex("dbo.WorkApprovalCopyUser", "ApprovalId");
            AddForeignKey("dbo.WorkApprovalCopyUser", "UserId", "dbo.SystemUser", "Id", cascadeDelete: true);
            AddForeignKey("dbo.WorkApprovalCopyUser", "ApprovalId", "dbo.WorkApproval", "Id", cascadeDelete: true);
        }
    }
}
