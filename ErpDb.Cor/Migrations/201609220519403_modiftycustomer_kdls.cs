namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modiftycustomer_kdls : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customer", "CustomerType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customer", "CustomerType", c => c.Short(nullable: false));
        }
    }
}
