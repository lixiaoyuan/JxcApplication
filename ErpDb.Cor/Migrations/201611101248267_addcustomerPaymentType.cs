namespace BusinessDb.Cor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcustomerPaymentType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "PaymentType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customer", "PaymentType");
        }
    }
}
