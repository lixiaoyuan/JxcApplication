namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProducingDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductInStorageDetail", "ProducingDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductInStorageDetail", "ProducingDate");
        }
    }
}
