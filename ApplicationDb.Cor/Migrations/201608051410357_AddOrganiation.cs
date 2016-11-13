namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrganiation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ParentId = c.Guid(),
                        Code = c.String(maxLength: 20, unicode: false),
                        Name = c.String(maxLength: 20, unicode: false),
                        Sort = c.Int(nullable: false),
                        Enable = c.Boolean(nullable: false),
                        Checked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organization", t => t.ParentId)
                .Index(t => t.ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organization", "ParentId", "dbo.Organization");
            DropIndex("dbo.Organization", new[] { "ParentId" });
            DropTable("dbo.Organization");
        }
    }
}
