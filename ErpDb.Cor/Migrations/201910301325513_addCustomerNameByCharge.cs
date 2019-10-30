namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCustomerNameByCharge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Charge", "CustomerName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Charge", "CustomerName");
        }
    }
}
