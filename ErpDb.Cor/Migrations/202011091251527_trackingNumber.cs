namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trackingNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductOutStorage", "TrackingNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductOutStorage", "TrackingNumber");
        }
    }
}
