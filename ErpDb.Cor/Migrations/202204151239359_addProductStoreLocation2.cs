namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductStoreLocation2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductOutStorageDetail", "StoreLocation", c => c.String());
            AddColumn("dbo.ProductOutStorageDetail", "StoreLocationCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductOutStorageDetail", "StoreLocationCode");
            DropColumn("dbo.ProductOutStorageDetail", "StoreLocation");
        }
    }
}
