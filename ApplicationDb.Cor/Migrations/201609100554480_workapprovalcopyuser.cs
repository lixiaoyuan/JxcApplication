namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class workapprovalcopyuser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkApprovalCopyUser",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        WorkApprovalId = c.Guid(nullable: false),
                        Sort = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SystemUser", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.WorkApproval", t => t.WorkApprovalId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.WorkApprovalId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkApprovalCopyUser", "WorkApprovalId", "dbo.WorkApproval");
            DropForeignKey("dbo.WorkApprovalCopyUser", "UserId", "dbo.SystemUser");
            DropIndex("dbo.WorkApprovalCopyUser", new[] { "WorkApprovalId" });
            DropIndex("dbo.WorkApprovalCopyUser", new[] { "UserId" });
            DropTable("dbo.WorkApprovalCopyUser");
        }
    }
}
