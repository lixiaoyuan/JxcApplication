namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRibbonNode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthRibbonNode",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PatentId = c.Guid(),
                        DisplayName = c.String(maxLength: 200, unicode: false),
                        Caption = c.String(maxLength: 100, unicode: false),
                        RibbonNodeRootId = c.Guid(),
                        NodeType = c.Int(nullable: false),
                        Image = c.String(maxLength: 500, unicode: false),
                        LinkName = c.String(maxLength: 200, unicode: false),
                        Sort = c.Short(nullable: false),
                        Enable = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuthRibbonNode", t => t.PatentId)
                .Index(t => t.PatentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthRibbonNode", "PatentId", "dbo.AuthRibbonNode");
            DropIndex("dbo.AuthRibbonNode", new[] { "PatentId" });
            DropTable("dbo.AuthRibbonNode");
        }
    }
}
