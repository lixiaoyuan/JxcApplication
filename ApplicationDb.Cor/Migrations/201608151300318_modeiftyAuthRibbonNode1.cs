namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modeiftyAuthRibbonNode1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuthRibbonNode", "Level", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuthRibbonNode", "Level");
        }
    }
}
