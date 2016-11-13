namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatesome : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkApprovalOrderCopyUser",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        WorkApprovalOrderId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        IsSeed = c.Boolean(),
                        Comment = c.String(maxLength: 300, unicode: false),
                        ModiftyTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SystemUser", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.WorkApprovalOrder", t => t.WorkApprovalOrderId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.WorkApprovalOrderId);
            
            CreateTable(
                "dbo.WorkApprovalOrderUser",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        WorkApprovalOrderId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Result = c.Int(nullable: false),
                        Comment = c.String(maxLength: 300, unicode: false),
                        ModiftyTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SystemUser", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.WorkApprovalOrder", t => t.WorkApprovalOrderId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.WorkApprovalOrderId);
            
            DropColumn("dbo.WorkApprovalOrder", "ApprovalUsers");
            DropColumn("dbo.WorkApprovalOrder", "CopyToUsers");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WorkApprovalOrder", "CopyToUsers", c => c.String(maxLength: 6000, unicode: false));
            AddColumn("dbo.WorkApprovalOrder", "ApprovalUsers", c => c.String(maxLength: 6000, unicode: false));
            DropForeignKey("dbo.WorkApprovalOrderUser", "WorkApprovalOrderId", "dbo.WorkApprovalOrder");
            DropForeignKey("dbo.WorkApprovalOrderUser", "UserId", "dbo.SystemUser");
            DropForeignKey("dbo.WorkApprovalOrderCopyUser", "WorkApprovalOrderId", "dbo.WorkApprovalOrder");
            DropForeignKey("dbo.WorkApprovalOrderCopyUser", "UserId", "dbo.SystemUser");
            DropIndex("dbo.WorkApprovalOrderUser", new[] { "WorkApprovalOrderId" });
            DropIndex("dbo.WorkApprovalOrderUser", new[] { "UserId" });
            DropIndex("dbo.WorkApprovalOrderCopyUser", new[] { "WorkApprovalOrderId" });
            DropIndex("dbo.WorkApprovalOrderCopyUser", new[] { "UserId" });
            DropTable("dbo.WorkApprovalOrderUser");
            DropTable("dbo.WorkApprovalOrderCopyUser");
        }
    }
}
