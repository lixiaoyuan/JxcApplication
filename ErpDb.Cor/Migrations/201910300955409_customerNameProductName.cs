namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class customerNameProductName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductOutStorageDetail", "ProductName", c => c.String());
            AddColumn("dbo.ProductOutStorage", "CustomerName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductOutStorage", "CustomerName");
            DropColumn("dbo.ProductOutStorageDetail", "ProductName");
        }
    }
}
