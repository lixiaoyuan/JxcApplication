namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modeiftyAuthRibbonNode2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuthRibbonNode", "Checked", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuthRibbonNode", "Checked");
        }
    }
}
