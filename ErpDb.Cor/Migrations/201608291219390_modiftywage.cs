namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modiftywage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WageMap", "WageDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WageMap", "WageDate", c => c.DateTime(nullable: false));
        }
    }
}
