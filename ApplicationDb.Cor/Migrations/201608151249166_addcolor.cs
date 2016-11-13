namespace ApplicationDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcolor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuthRibbonNode", "Color", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuthRibbonNode", "Color");
        }
    }
}
