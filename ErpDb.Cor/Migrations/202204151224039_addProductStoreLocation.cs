namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductStoreLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "StoreLocation", c => c.String());
            AddColumn("dbo.Product", "StoreLocationCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "StoreLocationCode");
            DropColumn("dbo.Product", "StoreLocation");
        }
    }
}
