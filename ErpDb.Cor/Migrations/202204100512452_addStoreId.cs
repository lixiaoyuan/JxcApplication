namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addStoreId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductOutStorageDetail", "StoreId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductOutStorageDetail", "StoreId");
        }
    }
}
