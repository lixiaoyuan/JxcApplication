namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomerType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "CustomerType", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "CustomerType");
        }
    }
}
