namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSort : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductOutStorageDetail", "SortId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductOutStorageDetail", "SortId");
        }
    }
}
