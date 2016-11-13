namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addWorkApproval : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkApproval",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 20, unicode: false),
                        Enable = c.Boolean(nullable: false),
                        Remark = c.String(maxLength: 100, unicode: false),
                        FormDataTemplate = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkApprovalCopyUser",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        ApprovalId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ApprovalId })
                .ForeignKey("dbo.WorkApproval", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.SystemUser", t => t.ApprovalId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ApprovalId);
            
            CreateTable(
                "dbo.WorkApprovalUser",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        ApprovalId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ApprovalId })
                .ForeignKey("dbo.WorkApproval", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.SystemUser", t => t.ApprovalId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ApprovalId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkApprovalUser", "ApprovalId", "dbo.SystemUser");
            DropForeignKey("dbo.WorkApprovalUser", "UserId", "dbo.WorkApproval");
            DropForeignKey("dbo.WorkApprovalCopyUser", "ApprovalId", "dbo.SystemUser");
            DropForeignKey("dbo.WorkApprovalCopyUser", "UserId", "dbo.WorkApproval");
            DropIndex("dbo.WorkApprovalUser", new[] { "ApprovalId" });
            DropIndex("dbo.WorkApprovalUser", new[] { "UserId" });
            DropIndex("dbo.WorkApprovalCopyUser", new[] { "ApprovalId" });
            DropIndex("dbo.WorkApprovalCopyUser", new[] { "UserId" });
            DropTable("dbo.WorkApprovalUser");
            DropTable("dbo.WorkApprovalCopyUser");
            DropTable("dbo.WorkApproval");
        }
    }
}
