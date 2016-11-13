namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addworkapprovaluser : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.WorkApprovalUser");
            CreateTable(
                "dbo.WorkApprovalUser",
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
            DropForeignKey("dbo.WorkApprovalUser", "WorkApprovalId", "dbo.WorkApproval");
            DropForeignKey("dbo.WorkApprovalUser", "UserId", "dbo.SystemUser");
            DropIndex("dbo.WorkApprovalUser", new[] { "WorkApprovalId" });
            DropIndex("dbo.WorkApprovalUser", new[] { "UserId" });
            DropTable("dbo.WorkApprovalUser");
        }
    }
}
