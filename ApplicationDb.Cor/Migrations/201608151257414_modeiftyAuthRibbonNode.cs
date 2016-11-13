namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modeiftyAuthRibbonNode : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AuthRibbonNode", "PatentId", "dbo.AuthRibbonNode");
            DropIndex("dbo.AuthRibbonNode", new[] { "PatentId" });
            RenameColumn(table: "dbo.AuthRibbonNode", name: "PatentId", newName: "ParentId");
            CreateIndex("dbo.AuthRibbonNode", "ParentId");
            AddForeignKey("dbo.AuthRibbonNode", "ParentId", "dbo.AuthRibbonNode", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthRibbonNode", "ParentId", "dbo.AuthRibbonNode");
            DropIndex("dbo.AuthRibbonNode", new[] { "ParentId" });
            RenameColumn(table: "dbo.AuthRibbonNode", name: "ParentId", newName: "PatentId");
            CreateIndex("dbo.AuthRibbonNode", "PatentId");
            AddForeignKey("dbo.AuthRibbonNode", "PatentId", "dbo.AuthRibbonNode", "Id");
        }
    }
}
