namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productOutStorageAddPayType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductOutStorage", "PaymoneyType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductOutStorage", "PaymoneyType");
        }
    }
}
