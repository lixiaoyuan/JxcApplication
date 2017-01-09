namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOrgWorkApproval : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationWorkApprovals",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrganizationId = c.Guid(nullable: false),
                        WorkApprovalId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.WorkApproval", "DisplayName", c => c.String(maxLength: 40, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkApproval", "DisplayName");
            DropTable("dbo.OrganizationWorkApprovals");
        }
    }
}
