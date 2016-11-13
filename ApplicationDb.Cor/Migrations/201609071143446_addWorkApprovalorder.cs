namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWorkApprovalorder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkApprovalOrder",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        WorkApprovalId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        UserName = c.String(maxLength: 20, unicode: false),
                        OrderStatus = c.Int(nullable: false),
                        OrderResult = c.Int(nullable: false),
                        StartingTime = c.DateTime(nullable: false),
                        StopTime = c.DateTime(nullable: false),
                        ApprovalUsers = c.String(maxLength: 6000, unicode: false),
                        NextApprovalUserId = c.Guid(),
                        CopyToUsers = c.String(maxLength: 6000, unicode: false),
                        Comment = c.String(maxLength: 6000, unicode: false),
                        FormData = c.Binary(maxLength: 8000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WorkApproval", t => t.WorkApprovalId, cascadeDelete: true)
                .Index(t => t.WorkApprovalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkApprovalOrder", "WorkApprovalId", "dbo.WorkApproval");
            DropIndex("dbo.WorkApprovalOrder", new[] { "WorkApprovalId" });
            DropTable("dbo.WorkApprovalOrder");
        }
    }
}
