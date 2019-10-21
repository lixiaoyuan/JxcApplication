namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCustomeType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductOutStorage", "CustomerType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductOutStorage", "CustomerType");
        }
    }
}
