namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adduri : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuthRibbonNode", "IsDefaultPageCategory", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuthRibbonNode", "IsDefaultPageCategory");
        }
    }
}
