namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrganizationUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrganizationUser",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OrganizationId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrganizationUser");
        }
    }
}
